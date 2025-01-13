
const Basket = addEventListener("load", async () => {
    DrawBacket()
})

const DrawBacket = async () => {
    let products = JSON.parse(sessionStorage.getItem("basket"))
    for (let i = 0; i < products.length; i++)
        await showProductBasket(products[i])

}

const showProductBasket = async (product) => {
    const inbasket = await fetch(`api/product/${product}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },

        query: {
            id: product
        }


    });
    shopingBag = await inbasket.json();
    console.log(shopingBag)
    showOneProduct(shopingBag);

}
const showOneProduct = async (product) => {
    const url = `./Images/${product.image}`
    let tmp = document.getElementById("temp-row");
    let cloneProduct = tmp.content.cloneNode(true)
    cloneProduct.querySelector(".image").style.backgroundImage = `url(${url})`
    cloneProduct.querySelector(".descriptionColumn").textContent = product.descreption
    cloneProduct.querySelector(".availabilityColumn").innerText = true;
    document.querySelector("tbody").appendChild(cloneProduct)
};

const detials = () => {
    let UserId = JSON.parse(sessionStorage.getItem("userId"))
    let orderItems1 = JSON.parse(sessionStorage.getItem("basket"))
    const OrderItems = []
    orderItems1.map(t => {
        let object = { productId: t, qantity: 1 }

        OrderItems.push(object)
    })

    let OrderSum = 100
    let OrderDate = "2025-01-05"
    /* OrderDate=OrderDate.toLocaleDateString()*/
    return ({
        OrderDate, OrderSum, UserId, OrderItems,
    })
}

const placeOrder = async () => {
    let alldetials = detials()
    const orderss = await fetch('api/Order', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },

        body: JSON.stringify(alldetials)

    });
    alldetialss = await orderss.json();
    if (orderss.ok) {
        alert("nice")
        sessionStorage.setItem("basket", JSON.stringify([]))
        location.reload()
        window.location.href = "Products.html";

    }


    else
        alert("😒")


}




