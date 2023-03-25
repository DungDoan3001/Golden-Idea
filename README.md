# GoldenIdea - A University Idea Collection System

This project is for the University of Greenwich and was written for the subject "COMP1640".

## Overview

This system is a secure web-enabled role-based platform for collecting ideas for improvement from staff in a large University.

## Related Link

- **The Application can be view at:** [GOLDENIDEA FRONT PAGE](https://goldenidea.dungdoan.me/) - OR - [MIRROR](https://golden-idea-comp-1640.vercel.app/)
- **API Deffinition can be view at:** [API DEFFINITION](https://goldenidea.azurewebsites.net/swagger/index.html)

## Roles

- **Quality Assurance Manager:** Oversees the process.
- **Department QA Coordinator:** Manages the process for their department and encourages staff to contribute.
- **Staff (Academic and Support):** Can submit one or more ideas.

## Role-Based Access Control (RBAC)

| Role                         | Permissions                                                                                                                                                                                                     |
| ---------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Quality Assurance Manager    | Can view all ideas and comments. Can add or delete categories. Can download all data after final closure date.                                                                                                  |
| Department QA Coordinator    | Can view all ideas and comments in their department. Receives email notifications for new ideas in their department.                                                                                            |
| Staff (Academic and Support) | Can submit one or more ideas. Can view all submitted ideas and comment on them. Can give a Thumbs Up or Thumbs Down for any idea (once per idea). Receives email notifications for new comments on their ideas. |

## Features

| Feature               | Description                                                                                                                                                         |
| --------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Terms and Conditions  | Staff must agree to Terms and Conditions before submitting an idea.                                                                                                 |
| Document Upload       | Staff can optionally upload documents to support their ideas.                                                                                                       |
| Idea Categorization   | Ideas can be categorized (tagged) from a list of categories at submission.                                                                                          |
| Category Management   | The QA Manager can add or delete categories (if unused).                                                                                                            |
| Idea Viewing          | All staff can see all submitted ideas and comment on them. They can also give a Thumbs Up or Thumbs Down for any idea (once per idea).                              |
| Real-time Comments    | Comments are updated in real-time using SignalR integration.                                                                                                        |
| Anonymous Posting     | Ideas and comments can be posted anonymously (author details stored in database).                                                                                   |
| Closure Dates         | New ideas are disabled after a closure date but comments can continue until final closure date.                                                                     |
| Email Notifications   | Department QA Coordinators receive email notifications for new ideas in their department. Idea authors receive email notifications for new comments on their ideas. |
| Idea Lists            | Lists of Most Popular, Most Viewed, Latest Ideas, and Latest Comments are available to all users (paginated with 5 per page).                                       |
| Data Downloading      | The University QA Manager can download all data after final closure date in CSV format (uploaded documents in ZIP file).                                            |
| System Administration | An administrator maintains system data such as closure dates and staff details.                                                                                     |
| AI voice Chatbot | User can ask some FAQ or change color mode or search ideas by title or navigate to any page in this app via AI voice chatbot.                                                                                     |
| AI image generating | When user submit their ideas they can generate image with the title by using Generate image with Open AI Dall-E2.                                                                                     |
## Reports

### Statistics

- Number of ideas by department
- Percentage of ideas by department
- Number of contributors by department

### Exception Reports

- Ideas without comments
- Anonymous ideas and comments

## Statistical Analysis

Statistical analysis such as number of ideas per department is available.

## Technology Stack

The technology stack for this system includes:

- Front-end: React.js with Material UI framework, and SignalR integration
- Back-end: ASP.NET Core Web API with generic repository pattern, unit of work pattern, dependency injection, and SignalR integration
- Database: PostgreSQL

## Contributors

### Backend Development:

Doan Thanh Dung  
Bui Cong Thuan

### Frontend Development:

Tran Trung Tien  
Pham Hoa Hiep

