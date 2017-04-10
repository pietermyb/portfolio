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
    /// Skill Controller Class
    /// </summary>
    [Route("api/[controller]")]
    public class SkillsController : Controller
    {
        private PortfolioContext _appDdContext;
        private DbSet<Skill> _Skills;
        private ILogger _logger;

        /// <summary>
        /// Creates an instance of the Skills Controller class
        /// </summary>
        /// <param name="appDdContext"></param>
        /// <param name="loggerFactory"></param>
        public SkillsController(PortfolioContext appDdContext, ILoggerFactory loggerFactory)
        {
            _appDdContext = appDdContext;
            
            try
            {
                _Skills = _appDdContext.Skills;
            }
            catch (Exception ex)
            {
                _logger = loggerFactory.CreateLogger("Skill Groups");
                _logger.LogCritical(ex.Message);
                throw;
            }
        }

        // GET api/Skills
        /// <summary>
        /// Gets all the skill group records
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Skill> Get()
        {
            if (_Skills != null)
            {
                return _Skills;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a specific skill group record for the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/Skills/5
        [HttpGet("{id}")]
        public Skill Get(int id)
        {
            if (_Skills != null)
            {
                return _Skills.FirstOrDefault(e=>e.Id == id);
            }
            else
            {
                return null;
            }
        }
        
        // POST api/Skills
        /// <summary>
        /// Post a new Skill record to the api.
        /// </summary>
        /// <param name = "Skill" ></param >
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public void Post(Skill Skill)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!_Skills.Any(e => e.Id == Skill.Id))
                    {
                        _appDdContext.Skills.Add(Skill);
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
        /// Delete a Skill Group record to the api.
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/Skills/5
        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public void Delete(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_Skills.Any(e => e.Id == id))
                    {
                        var Skill = _Skills.FirstOrDefault(e => e.Id == id);
                        _appDdContext.Skills.Remove(Skill);
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