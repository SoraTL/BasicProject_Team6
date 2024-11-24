const slides = document.querySelectorAll('.banner-slide');
const totalSlides = slides.length;
const bannerContainer = document.querySelector('.banner-container');

for (let i = 0; i < totalSlides; i++) {
    const clone = slides[i].cloneNode(true);
    bannerContainer.appendChild(clone);
}

function showSlide(index) {
    bannerContainer.style.transition = 'transform 1s ease-in-out';
    bannerContainer.style.transform = `translateX(-${index * 100}%)`;
}

function nextSlide() {
    currentIndex++;

    if (currentIndex === totalSlides) {
        showSlide(currentIndex);

        setTimeout(() => {
            bannerContainer.style.transition = 'none';
            bannerContainer.style.transform = `translateX(0%)`;
            currentIndex = 0;
        }, 1000);
    } else {
        showSlide(currentIndex);
    }
}

setInterval(nextSlide, 7000);

document.addEventListener("DOMContentLoaded", function () {
    const thumbnails = document.querySelectorAll(".thumbnail-images img");
    const mainImage = document.querySelector(".main-image");

    if (thumbnails.length > 0 && mainImage) {
        thumbnails.forEach(thumbnail => {
            thumbnail.addEventListener("click", function () {
                // Đổi ảnh chính thành ảnh thumbnail đã chọn
                mainImage.src = this.src;

                // Xóa class active khỏi tất cả thumbnail
                thumbnails.forEach(thumb => thumb.classList.remove("active"));

                // Thêm class active vào thumbnail hiện tại
                this.classList.add("active");
            });
        });
    } else {
        console.error("Thumbnails or main image not found.");
    }
});


