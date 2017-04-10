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
    /// History Controller Class
    /// </summary>
    [Route("api/[controller]")]
    public class HistoryController : Controller
    {
        private PortfolioContext _appDdContext;
        private DbSet<History> _History;
        private ILogger _logger;

        /// <summary>
        /// Creates an instance of the History Controller class
        /// </summary>
        /// <param name="appDdContext"></param>
        /// <param name="loggerFactory"></param>
        public HistoryController(PortfolioContext appDdContext, ILoggerFactory loggerFactory)
        {
            _appDdContext = appDdContext;
            
            try
            {
                _History = _appDdContext.HistoryInfo;
            }
            catch (Exception ex)
            {
                _logger = loggerFactory.CreateLogger("History");
                _logger.LogCritical(ex.Message);
                throw;
            }
        }

        // GET api/History
        /// <summary>
        /// Gets all the History records
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<History> Get()
        {
            if (_History != null)
            {
                return _History.ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a specific History record for the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/History/5
        [HttpGet("{id}")]
        public History Get(int id)
        {
            if (_History != null)
            {
                return _History.FirstOrDefault(e=>e.Id == id);
            }
            else
            {
                return null;
            }
        }

        // POST api/History
        /// <summary>
        /// Post a new History record to the api.
        /// </summary>
        /// <param name = "history" ></param >
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public void Post(History history)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!_History.Any(e => e.Id == history.Id))
                    {
                        _appDdContext.HistoryInfo.Add(history);
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
        /// Delete a History record to the api.
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/History/5
        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public void Delete(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_History.Any(e => e.Id == id))
                    {

                        var History = _History.FirstOrDefault(e => e.Id == id);
                        _appDdContext.HistoryInfo.Remove(History);
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