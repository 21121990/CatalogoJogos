﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModel
{
    public class JogoViewModel
    {
        public Guid id { get; set; }
        public string Nome { get; set; }

        public string Produtora { get; set; }
        public double Preco { get; set; }
    }
}
