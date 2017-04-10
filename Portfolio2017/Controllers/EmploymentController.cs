using Microsoft.AspNetCore.Mvc;
using Portfolio.Data.Context;
using System.Collections.Generic;
using System;
using Portfolio.Model.Entities;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Portfolio.API.Controllers
{
    /// <summary>
    /// Employment Controller Class
    /// </summary>
    [Route("api/[controller]")]
    public class EmploymentController : Controller
    {
        private PortfolioContext _appDdContext;
        private DbSet<Employment> _employmentHistory;
        private ILogger _logger;

        /// <summary>
        /// Creates an instance of the Employment Controller class
        /// </summary>
        /// <param name="appDdContext"></param>
        /// <param name="loggerFactory"></param>
        public EmploymentController(PortfolioContext appDdContext, ILoggerFactory loggerFactory)
        {
            _appDdContext = appDdContext;
            
            try
            {
                _employmentHistory = _appDdContext.EmploymentHistory;
            }
            catch (Exception ex)
            {
                _logger = loggerFactory.CreateLogger("Employment History");
                _logger.LogCritical(ex.Message);
                throw;
            }
        }

        // GET api/Employment
        /// <summary>
        /// Gets all the Employment records
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Employment> Get()
        {
            if (_employmentHistory != null)
            {
                return _employmentHistory.ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a specific employment record for the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/Employment/5
        [HttpGet("{id}")]
        public Employment Get(int id)
        {
            if (_employmentHistory != null)
            {
                return _employmentHistory.FirstOrDefault(e=>e.Id == id);
            }
            else
            {
                return null;
            }
        }
        
        // POST api/Employment
        /// <summary>
        /// Post a new employment record to the api.
        /// </summary>
        /// <param name = "employment" ></param >
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public void Post(Employment employment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!_appDdContext.EmploymentHistory.Any(e => e.Id == employment.Id))
                    {
                        _appDdContext.EmploymentHistory.Add(employment);
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
        /// Delete a employment record to the api.
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/Employment/5
        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public void Delete(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_appDdContext.EmploymentHistory.Any(e => e.Id == id))
                    {

                        var employment = _employmentHistory.FirstOrDefault(e => e.Id == id);
                        _appDdContext.EmploymentHistory.Remove(employment);
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