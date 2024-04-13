using System.Drawing;

namespace QLBangHangA.Models
{
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public void AddItem(CartItem item, int colorId = 0, int sizeId = 0)
        {
            var existingItem = Items.FirstOrDefault(i =>i.ProductId== item.ProductId && i.ColorId == colorId  && i.SizeId == sizeId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }
        public void RemoveItem(int productId)
        {
            Items.RemoveAll(i => i.ProductId == productId);
        }



    }
}
