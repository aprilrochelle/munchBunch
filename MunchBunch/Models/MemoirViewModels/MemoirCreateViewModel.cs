using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MunchBunch.Models.MemoirViewModels
{
    public class MemoirCreateViewModel
    {
        public Memoir Memoir { get; set; }
        public MemoirCreateViewModel() {

        }

    }
}