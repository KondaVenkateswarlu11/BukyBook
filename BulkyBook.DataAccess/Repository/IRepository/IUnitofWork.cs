﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitofWork
    {
        public ICategoryRepository Category { get; }

        public ICoverTypeRepository CoverType { get; }

        public IProductRepository Product { get; }
        void Save();
    }
}
