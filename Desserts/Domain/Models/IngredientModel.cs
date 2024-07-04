using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Models
{
    public class IngredientModel
    {
        #region Properties
        public string Key { get; set; }
        public string Email { get; set; }
        public int Id { get; set; }
        public string Heading { get; set; }
        public double Weight { get; set; }
        public double DefaultWeight { get; set; }
        public double WeightForSingle { set; get; }
        public string UnitOfMeasure { get; set; }
        public bool IsHeadingVisible { get; set; }
        public bool IsIngVisible { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }
        public LocationModel Location { get; set; }
        public string DisplayName => $"{Weight} {UnitOfMeasure} {Heading}";
        #endregion

    }
}
