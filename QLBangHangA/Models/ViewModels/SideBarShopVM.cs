using PagedList.Core;
using QLBangHangA.Models.Entities;

namespace QLBangHangA.Models.ViewModels
{
    public class SideBarShopVM
    {
        public PagedList<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
