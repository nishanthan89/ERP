using ERP.Resource.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.Models
{
   public class HolidayBO
    {
        public CreateHolidayViewModel createHoliday { get; set; }
        public SearchHolidayViewModel searchHoliday { get; set; }
        public DetailsHolidayViewModel detailsHoliday { get; set; }

        public List<CreateHolidayViewModel> createHolidayList { get; set; }
        public List<DetailsHolidayViewModel> detailsHolidayList { get; set; }
        public object[] EventArray { get; set; }
    }
}
