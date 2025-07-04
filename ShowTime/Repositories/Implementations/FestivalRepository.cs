﻿using Microsoft.EntityFrameworkCore;
using ShowTime.Context;
using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;

namespace ShowTime.Repositories.Implementations
{
    public class FestivalRepository(ShowTimeDbContext context) : Repository<Festival>(context), IFestivalRepository
    {
        private readonly DbSet<Festival> _festivals = context.Festivals;
    }
}
