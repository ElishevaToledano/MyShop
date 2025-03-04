const productList = addEventListener("load", async () => {
    let categoryIdArr = JSON.parse(sessionStorage.getItem("categoryIds")) || [];
    sessionStorage.setItem("categoryIds", JSON.stringify(categoryIdArr))
    drawProducts()
    showAllCategories();
    let basketArr = JSON.parse(sessionStorage.getItem("basket")) || [];
    sessionStorage.setItem("basket", JSON.stringify(basketArr))
    document.querySelector("#ItemsCountText").innerHTML = basketArr.length
})

const getDetailsFromForm = async () => {
    document.getElementById("PoductList").innerHTML = ''
    let search = {
        nameSearch: document.querySelector("#nameSearch").value,
        minPrice: parseInt(document.querySelector("#minPrice").value),
        maxPrice: parseInt(document.querySelector("#maxPrice").value)
    }
    return search
}
const filterProducts = async () => {
    drawProducts()
}
const drawProducts = async () => {
    const categoryIds1 = JSON.parse(sessionStorage.getItem("categoryIds"))
    console.log(categoryIds1)
    let { nameSearch, minPrice, maxPrice } = await getDetailsFromForm()
    let url = `https://localhost:44379/api/products`
    if (nameSearch || minPrice || maxPrice || categoryIds1) {
        url += '?'
        if (nameSearch != '')
            url += `&desc=${nameSearch}`
        if (minPrice)
            url += `&minPrice=${minPrice}`
        if (maxPrice)
            url += `&maxPrice=${maxPrice}`
        if (categoryIds1 != []) {
            for (let i = 0; i < categoryIds1.length; i++) {
                url += `&categoryIds=${categoryIds1[i]}`
            }
        }
    }
    try { 
    const allProducts = await fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
        query: {
            desc: nameSearch,
            minPrice: minPrice,
            maxPrice: maxPrice,
            categoryIds: categoryIds1
        }
    });
        const dataProducts = await allProducts.json();

        //console.log('GET Data:', dataProducts)
        showAllProducts(dataProducts);
}
catch(error) {
    alert("error")
}
}
const showAllProducts = async (products) => {
    for (let i = 0; i < products.length; i++) {
        showOneProduct(products[i]);
    }
}

const showOneProduct = async (product) => {
    let tmp = document.getElementById("temp-card"); 
    let cloneProduct = tmp.content.cloneNode(true)
    if (product.image)
        cloneProduct.querySelector("img").src = "../images/" + product.image
    cloneProduct.querySelector("h1").textContent = product.productName
    cloneProduct.querySelector(".price").innerText = product.price
    cloneProduct.querySelector(".description").innerText = product.descriptions
    cloneProduct.querySelector("button").addEventListener('click', () => { addToCart(product) })
    document.getElementById("PoductList").appendChild(cloneProduct)
}
const addToCart = (product) => {

    if (sessionStorage.getItem("user")) {

        let productsInbasket = JSON.parse(sessionStorage.getItem("basket"))
        productsInbasket.push(product.productId)
        sessionStorage.setItem("basket", JSON.stringify(productsInbasket))
        document.querySelector("#ItemsCountText").innerHTML = productsInbasket.length
            }
    else {
        alert("please sign")
        window.location.href = "../html/LogIn.html"
    }

}


const showAllCategories = async () => {

    const allCategories1 = await fetch('https://localhost:44379/api/Categories', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },

    });
    allCategories = await allCategories1.json();
    for (let i = 0; i < allCategories.length; i++) {
        showOneCategory(allCategories[i]);

    }
}
const showOneCategory = async (category) => {

    let tmp = document.getElementById("temp-category");
    let cloneProduct = tmp.content.cloneNode(true)

    cloneProduct.querySelector(".OptionName").textContent = category.categoryName
    cloneProduct.querySelector(".opt").addEventListener('change', () => { filterCategory(category) })
    document.getElementById("categoryList").appendChild(cloneProduct)
}

const filterCategory = (category) => {
    let categories = JSON.parse(sessionStorage.getItem("categoryIds"))
    let a = categories.indexOf(category.categoryId)
    a == -1 ? categories.push(category.categoryId) : categories.splice(a, 1)
    sessionStorage.setItem("categoryIds", JSON.stringify(categories))
    console.log(categories)
    drawProducts()
}

const Logout = () => {
    sessionStorage.removeItem("user");
    let productsToRemove = []
    sessionStorage.setItem("basket", JSON.stringify(productsToRemove))
}
