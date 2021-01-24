﻿using EyE.Server.Controllers.Common;
using EyE.Server.Data;
using EyE.Shared.Enums;
using EyE.Shared.Helpers;
using EyE.Shared.Models.Common;
using EyE.Shared.Models.Common.Interfaces;
using EyE.Shared.Models.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EyE.Server.Controllers.Common
{
    public abstract class IMDbController<T> : Database<T> where T : IMDbModel, new()
    {
        public IMDbController(
            ApplicationDbContext db,
            IHttpClientFactory clientFactory)
            : base(db, clientFactory)
        {
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> PutAsync(T model)
        {
            IQueryable<T> items;
            //С папкой "CartoonSeriesMyChildhood" допустимы дубликаты
            if (model.FolderName == FolderNames.CartoonSeriesMyChildhood)
                items = GetItems().Where(i => i.FolderName == FolderNames.CartoonSeriesMyChildhood);
            else
                items = GetItems().Where(i => i.FolderName != FolderNames.CartoonSeriesMyChildhood);

            if (await items.FirstOrDefaultAsync(i => i.IMDbId == model.IMDbId) == null)
            {
                return await AddAsync(model);
            }

            return BadRequest("Объект уже существует");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateItemsAsync()
        {
            foreach (var currentItem in await GetItems().AsNoTracking().ToListAsync())
            {
                var newItem = await IMDbHelper<T>.GetIMDbModelAsync(currentItem.Link, ClientFactory.CreateClient("localClient"));
                newItem.Id = currentItem.Id;
                newItem.AddingDate = currentItem.AddingDate;
                newItem.FolderName = currentItem.FolderName;
                Db.Update(newItem);
                await Db.SaveChangesAsync();
            }

            return NoContent();
        }
    }
}