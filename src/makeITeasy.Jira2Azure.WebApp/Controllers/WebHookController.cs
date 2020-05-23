﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using makeITeasy.AzureDevops.Models;
using makeITeasy.AzureDevops.Services.Domains.ItemDomain.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace makeITeasy.Jira2Azure.WebApp.Controllers
{
    public class WebHookController : Controller
    {
        private readonly ILogger<WebHookController> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public WebHookController(ILogger<WebHookController> logger, IMapper mapper, IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpPost]
        public IActionResult Jira([FromBody]JiraWebHookReceiveMessage incomingMessage)
        {
            var eventType = _mapper.Map<ItemChangeEventType>(incomingMessage.IssueEventTypeName);
            var item = _mapper.Map<Item>(incomingMessage.Issue);

            _mediator.Publish(new ItemChangeCommand(eventType, item));

            return Ok();
        }
    }
}
