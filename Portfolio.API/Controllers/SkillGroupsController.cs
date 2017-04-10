using Microsoft.AspNetCore.Mvc;
using Portfolio.Data.Context;
using System.Collections.Generic;
using System;
using Portfolio.Model.Entities;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.Dto;
using Portfolio.API.Core;
using Portfolio.API.Filters;
using System.Diagnostics.CodeAnalysis;

namespace Portfolio.API.Controllers
{
    /// <summary>
    /// SkillGroup Controller Class
    /// </summary>
    [Route("api/[controller]")]
    public class SkillGroupsController : Controller
    {
        private PortfolioContext _appDdContext;
        private DbSet<SkillGroup> _SkillGroups;
        private ILogger _logger;

        /// <summary>
        /// Creates an instance of the SkillGroups Controller class
        /// </summary>
        /// <param name="appDdContext"></param>
        /// <param name="loggerFactory"></param>
        public SkillGroupsController(PortfolioContext appDdContext, ILoggerFactory loggerFactory)
        {
            _appDdContext = appDdContext;
            
            try
            {
                _SkillGroups = _appDdContext.SkillGroups;
            }
            catch (Exception ex)
            {
                _logger = loggerFactory.CreateLogger("Skill Groups");
                _logger.LogCritical(ex.Message);
                throw;
            }
        }

        // GET api/SkillGroups
        /// <summary>
        /// Gets all the skill group records
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<SkillGroupDto> Get()
        {
            if (_SkillGroups != null)
            {
                return _SkillGroups.Include(s => s.Skills).Select(s => s.Map());
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
        // GET api/SkillGroups/5
        [HttpGet("{id}")]
        public SkillGroupDto Get(int id)
        {
            if (_SkillGroups != null)
            {
                return _SkillGroups.Include(s => s.Skills).FirstOrDefault(e=>e.Id == id).Map();
            }
            else
            {
                return null;
            }
        }

        // POST api/SkillGroups
        /// <summary>
        /// Post a new SkillGroup record to the api.
        /// </summary>
        /// <param name = "skillGroup" ></param >
        [HttpPost]
        [ValidateModel, SuppressMessage("Security", "SG0016:Controller method is vulnerable to CSRF")]
        [Authorize(Policy = "SuperUsers")]
        public void Post([FromBody]SkillGroup skillGroup)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!_SkillGroups.Any(e => e.Id == skillGroup.Id))
                    {
                        _appDdContext.SkillGroups.Add(skillGroup);
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
        // DELETE api/SkillGroups/5
        [HttpDelete("{id}")]
        [ValidateModel, SuppressMessage("Security", "SG0016:Controller method is vulnerable to CSRF")]
        [Authorize(Policy = "SuperUsers")]
        public void Delete(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_SkillGroups.Any(e => e.Id == id))
                    {
                        var skillGroup = _SkillGroups.FirstOrDefault(e => e.Id == id);
                        _appDdContext.SkillGroups.Remove(skillGroup);
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