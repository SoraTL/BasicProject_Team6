USE HHL;
Create database HHL
-- Table: USERS
CREATE TABLE USERS (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(50) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    email NVARCHAR(100) NOT NULL UNIQUE,
    phone NVARCHAR(15),
    address NVARCHAR(255),
    role NVARCHAR(10) CHECK (role IN ('admin', 'customer')) NOT NULL,
    create_at DATETIME DEFAULT GETDATE(),
    update_at DATETIME DEFAULT GETDATE()
);

-- Table: CATEGORY
CREATE TABLE CATEGORY (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(50) NOT NULL,
    description NVARCHAR(MAX),
    create_at DATETIME DEFAULT GETDATE(),
    update_at DATETIME DEFAULT GETDATE()
);

-- Table: PRODUCTS
CREATE TABLE PRODUCTS (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    description NVARCHAR(MAX),
    price DECIMAL(10, 2) NOT NULL,
    quantity INT NOT NULL,
    category_id INT,
    create_at DATETIME DEFAULT GETDATE(),
    update_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (category_id) REFERENCES CATEGORY(id)
);

-- Table: ORDERS
CREATE TABLE ORDERS (
    id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT,
    status NVARCHAR(10) CHECK (status IN ('pending', 'processing', 'completed', 'canceled')) NOT NULL,
    create_at DATETIME DEFAULT GETDATE(),
    update_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES USERS(ID)
);

-- Table: ORDER_ITEMS
CREATE TABLE ORDER_ITEMS (
    id INT IDENTITY(1,1) PRIMARY KEY,
    order_id INT,
    product_id INT,
    quantity INT NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    create_at DATETIME DEFAULT GETDATE(),
    update_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (order_id) REFERENCES ORDERS(id),
    FOREIGN KEY (product_id) REFERENCES PRODUCTS(id)
);

-- Table: REVIEWS
CREATE TABLE REVIEWS (
    id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT,
    product_id INT,
    rating INT CHECK (rating >= 1 AND rating <= 5),
    comment NVARCHAR(MAX),
    create_at DATETIME DEFAULT GETDATE(),
    update_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES USERS(ID),
    FOREIGN KEY (product_id) REFERENCES PRODUCTS(id)
);

-- Table: PAYMENTS
CREATE TABLE PAYMENTS (
    id INT IDENTITY(1,1) PRIMARY KEY,
    order_id INT,
    payment_method NVARCHAR(15) CHECK (payment_method IN ('credit_card', 'paypal', 'bank_transfer', 'cash')) NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    status NVARCHAR(10) CHECK (status IN ('pending', 'completed', 'failed')) NOT NULL,
    create_at DATETIME DEFAULT GETDATE(),
    update_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (order_id) REFERENCES ORDERS(id)
);

-- Table: CART
CREATE TABLE CART (
    id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT,
    create_at DATETIME DEFAULT GETDATE(),
    update_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES USERS(ID)
);

-- Table: CART_ITEM
CREATE TABLE CART_ITEM (
    id INT IDENTITY(1,1) PRIMARY KEY,
    cart_id INT,
    product_id INT,
    quantity INT NOT NULL,
    create_at DATETIME DEFAULT GETDATE(),
    update_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (cart_id) REFERENCES CART(id),
    FOREIGN KEY (product_id) REFERENCES PRODUCTS(id)
);
CREATE TABLE SALES (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    description NVARCHAR(MAX),
    discount_type NVARCHAR(10) CHECK (discount_type IN ('percentage', 'fixed')) NOT NULL,
    min_order_value DECIMAL(10, 2) NOT NULL,
    max_uses INT DEFAULT NULL,
    uses INT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
);

-- Table: ORDER_SALES
CREATE TABLE ORDER_SALES (
    id INT IDENTITY(1,1) PRIMARY KEY,
    order_id INT NOT NULL,
    sale_id INT NOT NULL,
    discount_value DECIMAL(10, 2) NOT NULL,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (order_id) REFERENCES ORDERS(id),
    FOREIGN KEY (sale_id) REFERENCES SALES(id)
);

-- Table: ORDER_DISCOUNTS
CREATE TABLE ORDER_DISCOUNTS (
    id INT IDENTITY(1,1) PRIMARY KEY,
    order_id INT NOT NULL,
    discount_id INT NOT NULL,
    discount_value DECIMAL(10, 2) NOT NULL,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (order_id) REFERENCES ORDERS(id),
    FOREIGN KEY (discount_id) REFERENCES SALES(id)
);
