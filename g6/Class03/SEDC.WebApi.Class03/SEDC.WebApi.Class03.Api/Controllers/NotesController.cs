﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("get-paging")]
        public ActionResult<List<Note>> GetAllPaging(int page, int pageSize)
        {
            var request = Request;
            var notesPaged = notes.Skip(page * pageSize).Take(pageSize).ToList();

            return Ok(notesPaged);
        }

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
    }
}