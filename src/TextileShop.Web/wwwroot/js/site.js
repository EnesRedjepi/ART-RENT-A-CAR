document.addEventListener('DOMContentLoaded', function () {
    var mainImage = document.getElementById('galleryMain');
    var thumbnails = document.querySelectorAll('[data-gallery-src]');

    thumbnails.forEach(function (thumbnail) {
        thumbnail.addEventListener('click', function () {
            mainImage.src = thumbnail.getAttribute('data-gallery-src');
            thumbnails.forEach(function (other) { other.classList.remove('active'); });
            thumbnail.classList.add('active');
        });
    });
});
