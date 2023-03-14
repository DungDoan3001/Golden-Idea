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

namespace Web.Api.Services.ZipFile
{
    public class ZipFileService : IZipFileService
    {
        protected IAppDbContext _context;
        private readonly IMapper _mapper;
        public ZipFileService(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public FileZip GetZippedFile(DateTime deliveryDate)
        {
            var deliveries = GetDeliveriesForDate(deliveryDate);
            var csvFiles = deliveries
                .GroupBy(a => a.StoreName)
                .Select(store =>
                    ToCSVFile(store.ToList(), $"{store.Key} {deliveryDate:dd-MM-yyyy} - Delivery.csv"))
                .ToList();

            return ToZip(csvFiles);
        }

        public async Task<FileZip> ZipIdeasOfTopicExpired(Guid topicId)
        {
            try
            {
                var topic = await _context.Topics
                    .Where(x => x.Id == topicId && x.FinalClosureDate < DateTime.UtcNow)
                    .SingleAsync();
                var ideasOfTopic = await _context.Ideas
                    .Include(x => x.Category)
                    .Include(x => x.Views)
                    .Include(x => x.Reactions)
                    .Where(x => x.TopicId == topic.Id)
                    .ToListAsync();
                List<FileZip> dataToCSV = new List<FileZip>();
                List<IdeaResponseModel> result = _mapper.Map<List<IdeaResponseModel>>(ideasOfTopic);
                dataToCSV.Add(ToCSVFile(result, $"Ideas.csv")); //format response data

                return ToZip(dataToCSV);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private FileZip ToZip(List<FileZip> files)
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
                FileName = $"Idea Details of Topic.zip"
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
