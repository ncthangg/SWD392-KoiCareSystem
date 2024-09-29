	Create Database  FA24_SE1702_PRN221_G5_KoiCareSystematHome
	Drop Database  FA24_SE1702_PRN221_G5_KoiCareSystematHome

CREATE TABLE "categories"(
    "id" BIGINT NOT NULL,
    "name" NVARCHAR(255) NOT NULL,
    "description" NVARCHAR(255) NOT NULL,
    "created_at" DATETIME  NOT NULL,
    "updated_at" DATETIME  NOT NULL
);
ALTER TABLE
    "categories" ADD CONSTRAINT"categories_id_primary" PRIMARY KEY("id");
CREATE TABLE "blogs"(
    "blog_id" BIGINT NOT NULL,
    "title" NVARCHAR(255) NULL,
    "user_id" BIGINT NOT NULL,
    "content" VARCHAR(255) NULL,
    "created_at" DATETIME NULL,
    "updated_at" DATETIME NULL
);
ALTER TABLE
    "blogs" ADD CONSTRAINT"blogs_blog_id_primary" PRIMARY KEY("blog_id");
CREATE TABLE "order_item"(
    "id" BIGINT NOT NULL,
    "order_id" BIGINT NOT NULL,
    "product_id" BIGINT NOT NULL,
    "quantity" BIGINT NOT NULL,
    "price" BIGINT NOT NULL
);
ALTER TABLE
    "order_item" ADD CONSTRAINT"order_item_id_primary" PRIMARY KEY("id");
CREATE TABLE "products"(
    "product_id" BIGINT NOT NULL,
    "product_name" NVARCHAR(255) NULL,
    "product_type" NVARCHAR(255) NULL,
    "price" DECIMAL(10, 2) NULL,
    "stock_quantity" BIGINT NULL,
    "created_at" DATETIME NULL,
    "updated_at" DATETIME NULL,
    "category_id" BIGINT NOT NULL
);
ALTER TABLE
    "products" ADD CONSTRAINT"products_product_id_primary" PRIMARY KEY("product_id");
CREATE TABLE "role"(
    "id" BIGINT NOT NULL,
    "name" NVARCHAR(255) NOT NULL
);
ALTER TABLE
    "role" ADD CONSTRAINT"role_id_primary" PRIMARY KEY("id");
CREATE TABLE "orders"(
    "order_id" BIGINT  NOT NULL,
    "product_id" BIGINT NULL,
    "order_date" BIGINT NULL,
    "quantity" BIGINT NULL,
    "total_price" DECIMAL(10, 2) NULL,
    "created_at" DATETIME NULL,
    "updated_at" DATETIME NULL,
    "user_id" BIGINT NOT NULL
);
ALTER TABLE
    "orders" ADD CONSTRAINT"orders_order_id_primary" PRIMARY KEY("order_id");
CREATE TABLE "ponds"(
    "pond_id" BIGINT  NOT NULL,
    "pond_name" NVARCHAR(255) NULL,
    "image_url" NVARCHAR(255) NULL,
    "size" DECIMAL(10, 2) NULL,
    "depth" DECIMAL(10, 2) NULL,
    "volume" DECIMAL(10, 2) NULL,
    "drain_count" BIGINT NULL,
    "pump_capacity" DECIMAL(10, 2) NULL,
    "created_at" DATETIME NULL,
    "updated_at" DATETIME NULL,
    "user_id" BIGINT NOT NULL,
    "skimmer_count" BIGINT NOT NULL
);
ALTER TABLE
    "ponds" ADD CONSTRAINT"ponds_pond_id_primary" PRIMARY KEY("pond_id");
CREATE TABLE "thresholds"(
    "parameter_id" BIGINT NOT NULL,
    "parameter_name" BIGINT NOT NULL,
    "min_value" BIGINT NOT NULL,
    "max_value" BIGINT NOT NULL,
    "created_at" DATETIME NOT NULL,
    "updated_at" DATETIME NOT NULL
);
ALTER TABLE
    "thresholds" ADD CONSTRAINT"thresholds_parameter_id_primary" PRIMARY KEY("parameter_id");
CREATE TABLE "user"(
    "id" BIGINT NOT NULL,
    "email" NVARCHAR(255) NOT NULL,
    "pashword_hash" NVARCHAR(255) NOT NULL,
    "role_id" BIGINT NOT NULL
);
ALTER TABLE
    "user" ADD CONSTRAINT"user_id_primary" PRIMARY KEY("id");
CREATE TABLE "koi_fish"(
    "fish_id" BIGINT NOT NULL,
    "fish_name" NVARCHAR(255) NULL,
    "user_id" BIGINT NOT NULL,
    "pond_id" BIGINT NULL,
    "image_url" NVARCHAR(255) NULL,
    "body_shape" NVARCHAR(255) NULL,
    "age" BIGINT NULL,
    "size" DECIMAL(10, 2) NULL,
    "weight" DECIMAL(10, 2) NULL,
    "gender" NVARCHAR(255) NULL,
    "breed" NVARCHAR(255) NULL,
    "origin" NVARCHAR(255) NULL,
    "price" DECIMAL(10, 2) NULL,
    "created_at" DATETIME NULL,
    "updated_at" DATETIME NULL
);
ALTER TABLE
    "koi_fish" ADD CONSTRAINT"koi_fish_fish_id_primary" PRIMARY KEY("fish_id");
CREATE TABLE "water_parameters"(
    "parameter_id" BIGINT NOT NULL,
    "pond_id" BIGINT NULL,
    "measurement_date" BIGINT NULL,
    "temperature" DECIMAL(10, 2) NULL,
    "salinity" DECIMAL(10, 2) NULL,
    "ph" DECIMAL(10, 2) NULL,
    "o2" DECIMAL(10, 2) NULL,
    "no2" DECIMAL(10, 2) NULL,
    "no3" DECIMAL(10, 2) NULL,
    "po4" DECIMAL(10, 2) NULL,
    "created_at" DATETIME NULL,
    "updated_at" DATETIME NULL
);
ALTER TABLE
    "water_parameters" ADD CONSTRAINT"water_parameters_parameter_id_primary" PRIMARY KEY("parameter_id");
CREATE TABLE "koi_growth_logs"(
    "log_id" BIGINT NOT NULL,
    "fish_id" BIGINT NULL,
    "growth_date" BIGINT NULL,
    "size" DECIMAL(10, 2) NULL,
    "weight" DECIMAL(10, 2) NULL,
    "created_at" DATETIME NULL,
    "updated_at" DATETIME NULL,
    "food_recomment" DECIMAL(8, 2) NOT NULL
);
ALTER TABLE
    "koi_growth_logs" ADD CONSTRAINT"koi_growth_logs_log_id_primary" PRIMARY KEY("log_id");
CREATE TABLE "payment"(
    "id" BIGINT NOT NULL,
    "order_id" BIGINT NOT NULL,
    "total" DECIMAL(8, 2) NOT NULL,
    "user_id" BIGINT NOT NULL
);
ALTER TABLE
    "payment" ADD CONSTRAINT"payment_id_primary" PRIMARY KEY("id");
ALTER TABLE
    "payment" ADD CONSTRAINT"payment_order_id_foreign" FOREIGN KEY("order_id") REFERENCES "orders"("order_id");
ALTER TABLE
    "orders" ADD CONSTRAINT"orders_user_id_foreign" FOREIGN KEY("user_id") REFERENCES "user"("id");
ALTER TABLE
    "products" ADD CONSTRAINT"products_category_id_foreign" FOREIGN KEY("category_id") REFERENCES "categories"("id");
ALTER TABLE
    "koi_fish" ADD CONSTRAINT"koi_fish_pond_id_foreign" FOREIGN KEY("pond_id") REFERENCES "ponds"("pond_id");
ALTER TABLE
    "payment" ADD CONSTRAINT"payment_user_id_foreign" FOREIGN KEY("user_id") REFERENCES "user"("id");
ALTER TABLE
    "order_item" ADD CONSTRAINT"order_item_order_id_foreign" FOREIGN KEY("order_id") REFERENCES "orders"("order_id");
ALTER TABLE
    "blogs" ADD CONSTRAINT"blogs_user_id_foreign" FOREIGN KEY("user_id") REFERENCES "user"("id");
ALTER TABLE
    "koi_fish" ADD CONSTRAINT"koi_fish_user_id_foreign" FOREIGN KEY("user_id") REFERENCES "user"("id");
ALTER TABLE
    "user" ADD CONSTRAINT"user_role_id_foreign" FOREIGN KEY("role_id") REFERENCES "role"("id");
ALTER TABLE
    "water_parameters" ADD CONSTRAINT"water_parameters_pond_id_foreign" FOREIGN KEY("pond_id") REFERENCES "ponds"("pond_id");
ALTER TABLE
    "ponds" ADD CONSTRAINT"ponds_user_id_foreign" FOREIGN KEY("user_id") REFERENCES "user"("id");
ALTER TABLE
    "order_item" ADD CONSTRAINT"order_item_product_id_foreign" FOREIGN KEY("product_id") REFERENCES "products"("product_id");
ALTER TABLE
    "koi_growth_logs" ADD CONSTRAINT"koi_growth_logs_fish_id_foreign" FOREIGN KEY("fish_id") REFERENCES "koi_fish"("fish_id");