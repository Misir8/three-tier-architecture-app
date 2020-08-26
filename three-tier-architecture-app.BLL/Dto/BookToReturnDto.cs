﻿using System;
using System.Collections.Generic;
using three_tier_architecture_app.DAL.Entities;

namespace three_tier_architecture_app.BLL.Dto
{
    public class BookToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime BirthDate { get; set; }
        public List<string> GenreName { get; set; }
    }
}