using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using System.IO;
using System;
using Web.Api.Controllers;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Data.Repository;
using Web.Api.Data.Context;
using Microsoft.EntityFrameworkCore;
using Web.Api.Entities;
using AutoMapper;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Services.Chart;

namespace Web.Api.Services.ZipFile
{
    public class ZipFileService : IZipFileService
    {
        protected IAppDbContext _context;
        private readonly IMapper _mapper;
        public readonly IChartService _chartService;
        public ZipFileService(IAppDbContext context, IMapper mapper, IChartService chartService)
        {
            _context = context;
            _mapper = mapper;
            _chartService = chartService;
        }

        public async Task<FileZip> ZipIdeasOfTopicExpired(Guid topicId)
        {
            try
            {
                var topic = await _context.Topics
                    .Where(x => x.Id == topicId && x.FinalClosureDate < DateTime.UtcNow)
                    .SingleAsync();
                if(topic == null)
                {
                    throw new Exception("Can not find the topic which is closed!");
                }
                var ideasOfTopic = await _context.Ideas
                    .Include(x => x.User)
                    .Include(x => x.Category)
                    .Include(x => x.Views)
                    .Include(x => x.Reactions)
                    .Where(x => x.TopicId == topic.Id)
                    .ToListAsync();
                List<FileZip> dataToCSV = new List<FileZip>();
                var result = _mapper.Map<List<IdeaForZipResponseModel>>(ideasOfTopic);
                dataToCSV.Add(ToCSVFile(result, $"Ideas.csv"));
                return ToZip(dataToCSV, "Idea Details of Topic");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<FileZip> ZipDashboardData()
        {
            try
            {
                var ideas = await _chartService.GetIdeasForChart();
                var totalIdeaCount = await _chartService.GetTotalIdeaOfEachDepartment();
                var totalStaffIdeaTopicComment = await _chartService.GetTotalOfStaffAndIdeaAndTopicAndCommment();
                var percentageOfIdea = await _chartService.GetPercentageOfIdeaForEachDepartments();
                List<TotalStaffAndIdeaAndTopicAndCommentResponseModel> totalStaffIdeaTopicComment_list = new List<TotalStaffAndIdeaAndTopicAndCommentResponseModel>();
                totalStaffIdeaTopicComment_list.Add(totalStaffIdeaTopicComment);
                List<FileZip> dataToCSV = new List<FileZip>
                {
                    ToCSVFile(ideas, $"All Ideas.csv"),
                    ToCSVFile(totalIdeaCount, $"Total Idea count for each department.csv"),
                    ToCSVFile(totalStaffIdeaTopicComment_list, $"Total Staff-Idea-Topic-Commment.csv"),
                    ToCSVFile(percentageOfIdea, $"Percentage of idea for each departments.csv")
                };
                return ToZip(dataToCSV, "Dashboard");
            }
            catch (Exception)
            {
                throw;
            }
        }
        private FileZip ToZip(List<FileZip> files, string fileZipName)
        {
            var compressedFileStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Create, true))
            {
                foreach (var file in files)
                {
                    var zipEntry = zipArchive.CreateEntry(file.FileName);

                    using (var originalFileStream = new MemoryStream(file.Bytes))
                    using (var zipEntryStream = zipEntry.Open())
                    {
                        originalFileStream.CopyTo(zipEntryStream);
                    }
                }
            }
            return new FileZip()
            {
                Bytes = compressedFileStream.ToArray(),
                FileName = $"{fileZipName.TrimEnd()}.zip"
            };
        }
        private FileZip ToCSVFile(IEnumerable records, string fileName)
        {
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                using (var writer = new StreamWriter(ms))
                {
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteRecords(records);
                    }
                }

                bytes = ms.ToArray();
            }

            return new FileZip
            {
                Bytes = bytes,
                FileName = fileName
            };
        }
        public class FileZip
        {
            public byte[] Bytes { get; set; }
            public string FileName { get; set; }
        }
    }
}
