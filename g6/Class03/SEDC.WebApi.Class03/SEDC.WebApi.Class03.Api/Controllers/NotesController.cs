﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEDC.WebApi.Class03.Api.Models;
using SEDC.WebApi.Class03.Api.Models.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEDC.WebApi.Class03.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        public static List<Note> notes = new List<Note>()
        {
            new Note(){ Text = "Don't forget to water the plant", Color = "blue", Tags = new List<Tag>()
                {
                    new Tag(){ Name = "Home", Color = "cyan"},
                    new Tag(){ Name = "Priority Low", Color = "green"}
                }
            },
            new Note(){ Text = "Drink more Tea", Color = "blue", Tags = new List<Tag>()
            {
                    new Tag(){ Name = "Misc", Color = "orange"},
                    new Tag(){ Name = "Priority Low", Color = "green"}
                }
            },
            new Note(){ Text = "Make a break every 1h of coding", Color = "blue", Tags = new List<Tag>()
            {
                    new Tag(){ Name = "work", Color = "blue"},
                    new Tag(){ Name = "Priority Medium", Color = "yellow"}
                }
            }
        };

        /// <summary>
        /// Gets all notes from DB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<Note>> Get()
        {
            return Ok(notes);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Note), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Note> GetById(int id)
        {
            try
            {
                return Ok(notes[id - 1]);
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound("No note with that id");
            }
            catch (Exception ex)
            {
                return BadRequest($"BROKEN REQUEST: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets notes with paging
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("get-paging")]
        public ActionResult<List<Note>> GetAllPaging(int page, int pageSize)
        {
            var request = Request;
            var notesPaged = notes.Skip(page * pageSize).Take(pageSize).ToList();

            return Ok(notesPaged);
        }

        /// <summary>
        /// Gets notes with paging
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("get-paging1")]
        public ActionResult<List<Note>> GetAllPaging1([FromQuery]GetPagingRouteParams request)
        {
            //debug only
            var request1 = Request;

            if (!request.IncludeTags)
            {
                var notePage = notes
                    .Skip(request.Page * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new Note { Color = x.Color, Text = x.Text, Tags = new List<Tag>() })
                    .ToList();

                return Ok(notePage);
            }

            var notesPaged = notes
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return Ok(notesPaged);
        }
    
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult Post([FromBody]Note request)
        {
            notes.Add(request);
            return Created(
                $"http://localhost:61925/api/notes/{notes.Count}",
                notes[notes.Count - 1]);
        }

        [HttpPost("post-note")]
        public ActionResult<List<Note>> Post1([FromBody]Note note, [FromQuery]GetPagingRouteParams query)
        {
            notes.Add(note);

            var notess = notes
                .Skip(query.Page * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            return Ok(notess);
        }

        [HttpGet("from-header-host")]
        public ActionResult<string> GetHeader([FromHeader]string host)
        {
            return Ok($"You are accessing from {host}");
        }

        [HttpGet("from-header-lang")]
        public ActionResult<string> GetHeader1(
            [FromHeader(Name = "Accept-Language")]string lang,
            [FromHeader]string host)
        {
            if (lang == "mk-MK")
            {
                return Ok($"Пристапувате до хост {host}");
            }

            return Ok($"You are accessing from {host}");
        }

        [HttpPost("post-from-form")]
        public ActionResult PostFromForm([FromForm]Note note)
        {
            notes.Add(note);
            return Ok(note);
        }
    }
}
