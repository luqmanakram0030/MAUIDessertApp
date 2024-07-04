using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Models
{
    public class InstructionModel
    {
        #region Properties
        public string Key { get; set; }
        public int Id { get; set; }
        public string Heading { get; set; }
        public double WeightForSingle { set; get; }
        public string UnitOfMeasure { get; set; }
        public string HeadingDescription { get; set; }
        public bool IsHeadingVisible { get; set; }
        public bool IsHeadingDescVisible { get; set; }
        public bool IsIngVisible { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }
        public string IngHeading { get; set; }
        public ObservableCollection<IngredientModel> IngredientList { get; set; }
        #endregion
    }
}
