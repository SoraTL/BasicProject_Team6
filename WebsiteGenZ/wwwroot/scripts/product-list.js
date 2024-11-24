document.querySelectorAll('.range-slider').forEach(slider => {
    slider.addEventListener('input', function () {
        const value = (this.value - this.min) / (this.max - this.min) * 100;
        this.style.background = `linear-gradient(to right, black 0%, black ${value}%, #e0e0e0 ${value}%)`;
    });
});

function toggleFilter(id) {
    const content = document.getElementById(id);
    content.classList.toggle('active');
}

function filterItems(filterId) {
    const input = document.querySelector(`#${filterId} .search-box input[type="text"]`);
    const filter = input.value.toLowerCase();
    const items = document.querySelectorAll(`#${filterId} .checkbox-list label`);

    items.forEach(item => {
        const text = item.textContent.toLowerCase();
        if (text.includes(filter)) {
            item.style.display = "flex";
        } else {
            item.style.display = "none";
        }
    });
}

const toInput = document.getElementById('to-price');
const toRange = document.getElementById('to-range');

// Function to update the text input based on the To range slider
function updateTextInputFromRange() {
    toInput.value = toRange.value;
    applyFilters(); // Load danh sách sản phẩm mới khi giá trị slider thay đổi
}

// Function to update the slider background fill from 0 to the To value
function updateSliderBackground() {
    const min = parseFloat(toRange.min);
    const max = parseFloat(toRange.max);
    const toPercentage = ((toRange.value - min) / (max - min)) * 100;

    toRange.style.background = `linear-gradient(to right, black 0%, black ${toPercentage}%, #e0e0e0 ${toPercentage}%)`;
}

// Function to gather filter values and load new product list
function applyFilters() {
    const fromPrice = document.getElementById('from-price').value;
    const toPrice = document.getElementById('to-price').value;
    const selectedBrands = Array.from(document.querySelectorAll('#brand-filter .checkbox-input:checked'))
        .map(checkbox => checkbox.value);
    const selectedCategories = Array.from(document.querySelectorAll('#category-filter .checkbox-input:checked'))
        .map(checkbox => checkbox.value);

    // Build query string for filter
    const queryParams = new URLSearchParams({
        fromPrice: fromPrice || null,
        toPrice: toPrice || null,
        brandIds: selectedBrands.join(','),
        categoryIds: selectedCategories.join(',')
    });

    // Fetch new product list from server
    fetch(`/ControllerName/Index?${queryParams.toString()}`, {
        method: 'GET',
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch product list');
            }
            return response.text(); // Expecting HTML fragment
        })
        .then(html => {
            const productListContainer = document.querySelector('.product-list-container');
            productListContainer.innerHTML = html; // Replace product list with new content
        })
        .catch(error => {
            console.error('Error loading product list:', error);
        });
}

// Apply filters automatically on checkbox or slider change
document.addEventListener('DOMContentLoaded', () => {
    // Add event listeners for brand checkboxes
    document.querySelectorAll('#brand-filter .checkbox-input').forEach(checkbox => {
        checkbox.addEventListener('change', applyFilters);
    });

    // Add event listeners for category checkboxes
    document.querySelectorAll('#category-filter .checkbox-input').forEach(checkbox => {
        checkbox.addEventListener('change', applyFilters);
    });

    // Update the slider background on page load
    updateSliderBackground();
});

// Event listener for range slider changes
toRange.addEventListener('input', updateTextInputFromRange);

// Initial setup
updateSliderBackground();
