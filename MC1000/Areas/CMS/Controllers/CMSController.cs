﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MC1000.Areas.CMS.Controllers
{
    [Area("CMS")]
    public class CMSController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}