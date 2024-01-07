using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using UvarTo.Application.Abstraction;
using UvarTo.Domain.Entities;
using UvarTo.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting.Internal;

namespace UvarTo.Application.Implementation
{
    internal class FoodmenuService : IFoodmenuService
    {
    }
}
