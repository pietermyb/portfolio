using Microsoft.AspNetCore.Mvc;
using Portfolio.Data.Context;
using System.Collections.Generic;
using System;
using Portfolio.Model.Entities;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.Filters;
using System.Diagnostics.CodeAnalysis;

namespace Portfolio.API.Controllers
{
    /// <summary>
    /// Project Controller Class
    /// </summary>
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private PortfolioContext _appDdContext;
        private DbSet<Project> _Project;
        private ILogger _logger;

        /// <summary>
        /// Creates an instance of the Project Controller class
        /// </summary>
        /// <param name="appDdContext"></param>
        /// <param name="loggerFactory"></param>
        public ProjectController(PortfolioContext appDdContext, ILoggerFactory loggerFactory)
        {
            _appDdContext = appDdContext;
            
            try
            {
                _Project = _appDdContext.Projects;
            }
            catch (Exception ex)
            {
                _logger = loggerFactory.CreateLogger("Project");
                _logger.LogCritical(ex.Message);
                throw;
            }
        }

        // GET api/Project
        /// <summary>
        /// Gets all the Project records
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Project> Get()
        {
            if (_Project != null)
            {
                return _Project.ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a specific Project record for the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/Project/5
        [HttpGet("{id}")]
        public Project Get(int id)
        {
            if (_Project != null)
            {
                return _Project.FirstOrDefault(e=>e.Id == id);
            }
            else
            {
                return null;
            }
        }

        // POST api/Project
        /// <summary>
        /// Post a new Project record to the api.
        /// </summary>
        /// <param name = "Project" ></param >
        [HttpPost]
        [ValidateModel, SuppressMessage("Security", "SG0016:Controller method is vulnerable to CSRF")]
        [Authorize(Policy = "SuperUsers")]
        public void Post([FromBody]Project Project)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!_Project.Any(e => e.Id == Project.Id))
                    {
                        _appDdContext.Projects.Add(Project);
                        _appDdContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                    {
                        _logger.LogCritical(ex.Message);
                    }
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete a Project record to the api.
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/Project/5
        [HttpDelete("{id}")]
        [ValidateModel, SuppressMessage("Security", "SG0016:Controller method is vulnerable to CSRF")]
        [Authorize(Policy = "SuperUsers")]
        public void Delete(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_Project.Any(e => e.Id == id))
                    {

                        var Project = _Project.FirstOrDefault(e => e.Id == id);
                        _appDdContext.Projects.Remove(Project);
                        _appDdContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                    {
                        _logger.LogCritical(ex.Message);
                    }
                    throw;
                }
            }
        }
    }
}