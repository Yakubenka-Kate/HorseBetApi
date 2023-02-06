﻿using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseBet.Presentation.Controllers
{
    [Route("api/bets")]
    [ApiController]
    public class BetController : ControllerBase
    {
        private readonly IServiceManager _service;

        public BetController(IServiceManager service) => _service = service;

        [HttpGet]
        public IActionResult GetBets()
        {
            var bets = _service.BetService.GetAllBets(trackChanges: false);

            return Ok(bets);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetBet(Guid entryId, Guid id)
        {
            var bet = _service.BetService.GetBetForEntry(entryId, id, trackChanges: false);

            return Ok(bet);
        }

        [HttpGet("{entryId}/entries")]
        public IActionResult GetBetsForEntry(Guid entryId)
        {
            var bets = _service.BetService.GetBetsForEntry(entryId, trackChanges: false);

            return Ok(bets);
        }

        [HttpPost]
        public IActionResult CreateBet( Guid entryId, [FromBody] BetForManipulationsDto bet)
        {
            if (bet is null)
                return BadRequest("Bet is null");

            var createdBet = _service.BetService.CreateBet(entryId, bet, trackChanges: false);

            return CreatedAtRoute(new { entryId, id = createdBet.Id }, createdBet);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteBetForEntry(Guid entryId, Guid id)
        {
            _service.BetService.DeleteBetForEntry(entryId, id, trackChanges: false);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateBetForEntry(Guid entryId, Guid id, [FromBody] BetForManipulationsDto bet)
        {
            if (bet is null)
                return BadRequest("Entry is null");

            _service.BetService.UpdateBetForEntry(entryId, id, bet, entryTrackChanges: false, betTrackChanges: true);

            return NoContent();
        }
    }
}