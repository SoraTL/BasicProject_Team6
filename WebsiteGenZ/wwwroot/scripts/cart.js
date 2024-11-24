function loadCart() {
    // Gửi yêu cầu AJAX để lấy giỏ hàng
    fetch('/Cart/Index')
        .then(response => response.text())
        .then(html => {
            // Hiển thị nội dung giỏ hàng (thay đổi DOM)
            document.getElementById('cart-container').innerHTML = html;
        })
        .catch(error => console.error('Error loading cart:', error));
}

document.addEventListener("DOMContentLoaded", function () {
    const incrementButtons = document.querySelectorAll(".increment-btn");
    const decrementButtons = document.querySelectorAll(".decrement-btn");
    const removeButtons = document.querySelectorAll(".remove-item");
    const quantityInputs = document.querySelectorAll(".quantity-input");

    function updateQuantity(cartItemId, quantity) +{
        return fetch('/Cart/UpdateQuantity', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ cartItemId, quantity })
        })
            .then(response => response.json());
    }

    function calculateSubtotalAndTotal() {
        const cartItems = document.querySelectorAll(".cart-item");
        let subtotal = 0;

        cartItems.forEach(item => {
            const quantity = parseInt(item.querySelector(".quantity-input").value);
            const price = parseFloat(item.querySelector(".price").getAttribute("data-price"));
            subtotal += quantity * price;
        });

        const estimatedTax = 50;
        const shippingHandling = 29;
        const total = subtotal + estimatedTax + shippingHandling;

        document.querySelector(".summary-details p span").textContent = `$${subtotal.toFixed(2)}`;
        document.querySelector(".total span").textContent = `$${total.toFixed(2)}`;
    }

    incrementButtons.forEach(button => {
        button.addEventListener("click", function () {
            const cartItemId = this.getAttribute("data-product-id");
            const quantityInput = document.querySelector(`.quantity-input[data-product-id='${cartItemId}']`);
            let currentQuantity = parseInt(quantityInput.value);
            const maxStock = parseInt(quantityInput.getAttribute("data-stock"));

            if (currentQuantity + 1 <= maxStock) {
                currentQuantity += 1;
                quantityInput.value = currentQuantity;
                updateQuantity(cartItemId, currentQuantity).then(result => {
                    if (!result.success) {
                        alert("Failed to update quantity: " + result.message);
                        quantityInput.value = currentQuantity - 1;
                    } else {
                        calculateSubtotalAndTotal();
                    }
                });
            } else {
                alert("Cannot add more than available stock.");
            }
        });
    });

    decrementButtons.forEach(button => {
        button.addEventListener("click", function () {
            const cartItemId = this.getAttribute("data-product-id");
            const quantityInput = document.querySelector(`.quantity-input[data-product-id='${cartItemId}']`);
            let currentQuantity = parseInt(quantityInput.value);

            if (currentQuantity > 1) {
                currentQuantity -= 1;
                quantityInput.value = currentQuantity;
                updateQuantity(cartItemId, currentQuantity).then(result => {
                    if (!result.success) {
                        alert("Failed to update quantity: " + result.message);
                        quantityInput.value = currentQuantity + 1;
                    } else {
                        calculateSubtotalAndTotal();
                    }
                });
            }
        });
    });

    quantityInputs.forEach(input => {
        let previousQuantity = parseInt(input.value);

        input.addEventListener("focus", function () {
            previousQuantity = parseInt(this.value);
        });

        input.addEventListener("blur", function () {
            const cartItemId = this.getAttribute("data-product-id");
            let newQuantity = parseInt(this.value);
            const maxStock = parseInt(this.getAttribute("data-stock"));

            if (isNaN(newQuantity) || newQuantity < 1 || newQuantity > maxStock) {
                alert("Invalid quantity or exceeds available stock.");
                this.value = previousQuantity;
            } else {
                updateQuantity(cartItemId, newQuantity).then(result => {
                    if (!result.success) {
                        alert("Failed to update quantity: " + result.message);
                        this.value = previousQuantity;
                    } else {
                        previousQuantity = newQuantity;
                        calculateSubtotalAndTotal();
                    }
                });
            }
        });
    });

    calculateSubtotalAndTotal();
});
