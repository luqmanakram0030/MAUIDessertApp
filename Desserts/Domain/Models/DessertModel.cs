using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Models
{
    public class DessertModel
    {
        #region Properties
        public string Key { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public string MakerName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsWindows { get; set; }
        public ObservableCollection<IngredientModel> IngredientModels { get; set; }
        public ObservableCollection<InstructionModel> InstructionsList { get; set; }
        public string Time { get; set; }
        public string Difficulty { get; set; }
        public string ImageSrc { get; set; }
        public string Video_Name { get; set; }
        public ImageSource Image { get; set; }
        public ImageSource FavImage { get; set; }
        public List<string> DessertImages { get; set; }
        public List<CommentsModel> Comments { get; set; }
        public List<BookMarkModel> BookMarks { get; set; }
        #endregion
    }

}
