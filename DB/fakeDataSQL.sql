INSERT INTO [dbo].[products] (product_id, product_name, product_type, price, stock_quantity, created_at, updated_at, category_id)
VALUES
(1, 'Wireless Mouse', 'Electronics', 25.99, 150, GETDATE(), GETDATE(), 101),
(2, 'Gaming Keyboard', 'Electronics', 89.99, 75, GETDATE(), GETDATE(), 101),
(3, 'Running Shoes', 'Apparel', 55.49, 200, GETDATE(), GETDATE(), 102)

INSERT INTO [dbo].[categories] (id, name, description, created_at, updated_at)
VALUES
(101, 'Electronics', 'Devices and gadgets like computers, phones, and accessories', GETDATE(), GETDATE()),
(102, 'Apparel', 'Clothing, shoes, and accessories', GETDATE(), GETDATE()),
(103, 'Stationery', 'Office and school supplies', GETDATE(), GETDATE())