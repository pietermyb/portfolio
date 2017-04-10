using Portfolio.Data.Context;
using Portfolio.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portfolio.Data
{
    public class PortfolioDbInitializer
    {
        private PortfolioContext context;
        private PortfolioIdentityContext identityContext;

        public PortfolioDbInitializer(IServiceProvider serviceProvider)
        {
            context = (PortfolioContext)serviceProvider.GetService(typeof(PortfolioContext));
            identityContext = (PortfolioIdentityContext)serviceProvider.GetService(typeof(PortfolioIdentityContext));
        }

        public void Seed()
        {
            //Ensure the Database exists and has the required tables
            if (identityContext != null)
            {
                identityContext.Database.EnsureCreated();
            }
            if (context != null)
            {
                context.Database.EnsureCreated();
            }

            if (!context.EmploymentHistory.Any())
            {
                var employment_01 = new Employment
                {
                    Id = 1,
                    Name = "Handmade Connections",
                    Description = "Worked as a website integrator, assisting with tying up the backend CMS with the designers HTML.",
                    ImageURL = "img/about/2.jpg"
                };

                var employment_02 = new Employment
                {
                    Id = 2,
                    Name = "Fundamental Software",
                    Description = "Assisted in the development of financial systems.",
                    ImageURL = "img/about/4.jpg"
                };

                var employment_03 = new Employment
                {
                    Id = 3,
                    Name = "Process Computing Technology",
                    Description = "Employed as the senior software developer and manager.",
                    ImageURL = "img/about/5.jpg"
                };

                var employment_04 = new Employment
                {
                    Id = 4,
                    Name = "EOH Microsoft Coastal",
                    Description = "Joined in October 2015. Employed as a Senior Software Developer on my path to Technical Lead.",
                    ImageURL = "img/about/6.jpg"
                };

                context.EmploymentHistory.Add(employment_01);
                context.EmploymentHistory.Add(employment_02);
                context.EmploymentHistory.Add(employment_03);
                context.EmploymentHistory.Add(employment_04);

                //context.SaveChanges();
            }

            if (!context.Projects.Any())
            {
                var project_01 = new Project
                {
                    Id = 1,
                    Name = "CODE PROJECT",
                    Description = "I write code articles from time to time about handy little utilities.",
                    Title = "SHARE",
                    ImageURL = "img/portfolio/codeproject.png"
                };

                var project_02 = new Project
                {
                    Id = 2,
                    Name = "FOUNDATION BACKBONE",
                    Description = "I assist with web administration and content",
                    Title = "VOLUNTEER",
                    ImageURL = "img/portfolio/foundation_backbone.jpg"
                };

                var project_03 = new Project
                {
                    Id = 3,
                    Name = "HOME AUTOMATION",
                    Description = "I love building little electrical kits to aid with home automation.",
                    Title = "TINKER",
                    ImageURL = "img/portfolio/homeautomation.png"
                };

                var project_04 = new Project
                {
                    Id = 4,
                    Name = "PSTIME GAMES",
                    Description = "I helped a friend convert a board-game into a iPhone store app.",
                    Title = "LEARN",
                    ImageURL = "img/portfolio/pstimegames.png"
                };

                context.Projects.Add(project_01);
                context.Projects.Add(project_02);
                context.Projects.Add(project_03);
                context.Projects.Add(project_04);

                //context.SaveChanges();
            }

            if (!context.SkillGroups.Any())
            {
                var skillsgroup_01 = new SkillGroup
                {
                    Id = 1,
                    Name = "Web Development",
                    Description = "MVC / ASP.NET web applications",
                    Skills = new List<Skill>
                    {
                        new Skill() { Id = 1, SkillGroupId = 1, Name = "MVVM with Knockout", Description = "" },
                        new Skill() { Id = 2, SkillGroupId = 1, Name = "CSS driven by LESS", Description = "" },
                        new Skill() { Id = 3, SkillGroupId = 1, Name = "Owin with Identity Management", Description = "" },
                        new Skill() { Id = 4, SkillGroupId = 1, Name = "Persistence with EF", Description = "" },
                        new Skill() { Id = 5, SkillGroupId = 1, Name = "API driven by WCF or Web API", Description = "" }
                    }
                };
                var skillsgroup_02 = new SkillGroup
                {
                    Id = 2,
                    Name = "Desktop Development",
                    Description = "WinForms desktop applications",
                    Skills = new List<Skill>
                    {
                        new Skill() { Id = 6, SkillGroupId = 2, Name = "Analysis & Consulting", Description = "" },
                        new Skill() { Id = 7, SkillGroupId = 2, Name = "API driven by WCF or Web API", Description = "" },
                        new Skill() { Id = 8, SkillGroupId = 2, Name = "Persistence with EF", Description = "" },
                        new Skill() { Id = 9, SkillGroupId = 2, Name = "Windows Service", Description = "" },
                        new Skill() { Id = 10, SkillGroupId = 2, Name = "Office and PDF integration", Description = "" }
                    }
                };
                var skillsgroup_03 = new SkillGroup
                {
                    Id = 3,
                    Name = "Mobile Development",
                    Description = "Apple, Android, Windows, Symbian",
                    Skills = new List<Skill>
                    {
                        new Skill() { Id = 11, SkillGroupId = 3, Name = "Home Automation", Description = "" },
                        new Skill() { Id = 12, SkillGroupId = 3, Name = "Cordova", Description = "" },
                        new Skill() { Id = 13, SkillGroupId = 3, Name = "Ionic", Description = "" },
                        new Skill() { Id = 14, SkillGroupId = 3, Name = "Carbide C++", Description = "" },
                        new Skill() { Id = 15, SkillGroupId = 3, Name = "IOS Dragonfire SDK", Description = "" }
                    }
                };

                context.SkillGroups.Add(skillsgroup_01);
                context.SkillGroups.Add(skillsgroup_02);
                context.SkillGroups.Add(skillsgroup_03);

                //context.SaveChanges();
            }

            if (!context.HistoryInfo.Any())
            {
                var history_01 = new History
                {
                    Id = 1,
                    Name = "Nelson Mandela Metropolitan University",
                    Description = "I attended Nelson Mandela Metropolitan University where I learned to code better than I use too. My studies taught me Java, VB.NET and C# as programming languages and in database programming I gaind skills in Oracle's PL-SQL and Microsoft SQL's T-SQL.",
                    ImageURL = "img/about/1.jpg",
                    CompanyURL = "https://www.nmmu.ac.za/",
                    EmploymentStartDate = new DateTime(2003, 01, 01),
                    EmploymentEndDate = new DateTime(2007, 01, 01)
                };

                var history_02 = new History
                {
                    Id = 2,
                    Name = "Handmade Connections",
                    Description = "After university HandMade Connections employed me as a junior developer where I coded in PHP. My main role was to assist with the integration of new HTML/CSS templates into the existing CMS. My skills in Web SEO also took a great leap forward.",
                    ImageURL = "img/about/2.jpg",
                    CompanyURL = "http://www.handmade.co.za",
                    EmploymentStartDate = new DateTime(2007, 01, 01),
                    EmploymentEndDate = new DateTime(2008, 01, 01)
                };

                var history_03 = new History
                {
                    Id = 3,
                    Name = "Process Computing Technology",
                    Description = "In March 2008 I moved to Cape Town where I joined PCT. I was employed as a developer and had to support legacy code in VB6 as well as maintain an existing ASP.NET system with codebehind written in VB.NET. Development got better when I helped integrate a ESRI GIS decision making system.",
                    ImageURL = "img/about/3.jpg",
                    CompanyURL = "http://www.pct.co.za",
                    EmploymentStartDate = new DateTime(2003, 01, 01),
                    EmploymentEndDate = new DateTime(2007, 01, 01)
                };

                var history_04 = new History
                {
                    Id = 4,
                    Name = "Fundamental Software",
                    Description = "I made a move to the city where I joined Fundamental Software. Here I learned a lot about SCRUM and Agile Programming. Nothing against finance but I soon required a challenge.",
                    ImageURL = "img/about/4.jpg",
                    CompanyURL = "http://www.fundamental.net/",
                    EmploymentStartDate = new DateTime(2003, 01, 01),
                    EmploymentEndDate = new DateTime(2007, 01, 01)
                };

                var history_05 = new History
                {
                    Id = 5,
                    Name = "PCT",
                    Description = "In May 2010 PCT makes me an offer to become the senior software developer and the developer team's Manager. Here I help the company to become Agile and we implement SCRUM throughout the company. An issue tracking system is installed and adopted.",
                    ImageURL = "img/about/5.jpg",
                    CompanyURL = "http://www.pct.co.za",
                    EmploymentStartDate = new DateTime(2003, 01, 01),
                    EmploymentEndDate = new DateTime(2007, 01, 01)
                };

                var history_06 = new History
                {
                    Id = 6,
                    Name = "EOH Microsoft Coastal",
                    Description = "In started working at EOH Microsoft Coastal in October 2015. So far I have consulted at Sanlam where we implemented a online claims portal. I'm currently consulting at Barclays CIB Digital.",
                    ImageURL = "img/about/6.jpg",
                    CompanyURL = "http://www.eohmc.co.za",
                    EmploymentStartDate = new DateTime(2003, 01, 01),
                    EmploymentEndDate = new DateTime(2007, 01, 01)
                };

                context.HistoryInfo.Add(history_01);
                context.HistoryInfo.Add(history_02);
                context.HistoryInfo.Add(history_03);
                context.HistoryInfo.Add(history_04);
                context.HistoryInfo.Add(history_05);
                context.HistoryInfo.Add(history_06);

                //context.SaveChanges();
            }

            context.SaveChanges();
        }
    }
}