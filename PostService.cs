using JobBoard.Application.Abstractions;
using JobBoard.Application.Abstractions.IService;
using JobBoard.Domain.Entities.DTOs;
using JobBoard.Domain.Entities.Models;
using Microsoft.AspNetCore.Http;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace JobBoard.Application.Services.BookService
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<string> Add(PostDTO postDto)
        {
            var post = new Post()
            {
                Job = postDto.Job,
                Requirements = postDto.Requirements,
                Salary = postDto.Salary,
                Contact = postDto.Contact
            };

            if(post != null)
            {
                await _postRepository.Create(post);
                return "Added";
            }
            return "Failed";
        }

        public async Task<string> Delete(int id)
        {
            var result =await _postRepository.Delete(x => x.ID == id);
            if (result)
            {
                return "Deleted";
            }
            else
            {
                return "Failed";
            }
           
        }

        public async Task<List<Post>> GetAllPost()
        {
            var result = await _postRepository.GetAll();
            return result.ToList();
        }
        
        public async Task<List<Post>> GetJobsBySalaryRange(decimal minSalary, decimal maxSalary)
        {
            var result = await _postRepository.GetAll();
    
            var filteredJobs = result
                .Where(post => 
                    decimal.TryParse(post.Salary, out var salaryValue) && 
                    salaryValue >= minSalary && 
                    salaryValue <= maxSalary)
                .ToList();

            return filteredJobs;
        }

        public async Task<string> AddAttachment(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return "No file selected.";
                }
                
                string uploadFolder = "path/to/uploaded/files/";
                
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                
                string fileName = Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadFolder, fileName);
                
                if (File.Exists(filePath))
                {
                    return $"{fileName} already exists.";
                }
                
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return $"{fileName} has been successfully uploaded.";
            }
            catch (Exception ex)
            {
                return $"File upload failed. Error: {ex.Message}";
            }
        }

        public async Task<Post> GetPostById(int id)
        {
            var result = await _postRepository.GetByAny(x=>x.ID == id);
            return result;
        }
        
        
        
        public async Task<string> Update(int id, PostDTO postDto)
        {
            var res = await _postRepository.GetByAny(x=> x.ID == id);
            
            if(res != null)
            {
                res.Job = postDto.Job;
                res.Requirements = postDto.Requirements;
                res.Salary = postDto.Salary;
                res.Contact = postDto.Contact;

                var result = await _postRepository.Update(res);
                if(result != null)
                {
                    return "Updated";
                }
                return "Failed";
            }
            return "Failed";
        }
        
        public async Task<string> GetJobsPDF()
        {
            var jobs = await _postRepository.GetAll();

            var text = string.Join("\n", jobs.Select(job =>
                $"{job.Job}|{job.Requirements}|{job.Salary}|{job.Contact}"));

            DirectoryInfo projectDirectoryInfo =
                Directory.GetParent(Environment.CurrentDirectory).Parent.Parent;

            var file = Guid.NewGuid().ToString();

            string pdfsFolder = Directory.CreateDirectory(
                Path.Combine(projectDirectoryInfo.FullName, "pdfs")).FullName;

            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(20));

                        page.Header()
                            .Text("Job Board - All Jobs")
                            .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                        page.Content()
                            .PaddingVertical(1, Unit.Centimetre)
                            .Column(x =>
                            {
                                x.Spacing(20);

                                x.Item().Text(text);
                            });

                        page.Footer()
                            .AlignCenter()
                            .Text(x =>
                            {
                                x.Span("Page ");
                                x.CurrentPageNumber();
                            });
                    });
                })
                .GeneratePdf(Path.Combine(pdfsFolder, $"{file}.pdf"));

            return Path.Combine(pdfsFolder, $"{file}.pdf");
        }
    }
}