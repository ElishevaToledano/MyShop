const Basket = addEventListener("load", async () => {
    DrawBasket()
})

const DrawBasket = async () => {
    let products = JSON.parse(sessionStorage.getItem("basket"))
    let totalAmount = 0;
    let productsToBasketTwo = []
    sessionStorage.setItem("basketTwo", JSON.stringify(productsToBasketTwo))
    for (let i = 0; i < products.length; i++)
    { 
        await showProductBasket(products[i])

    }
    let productsToBasket2 = JSON.parse(sessionStorage.getItem("basketTwo"))

    for (j = 0; j < productsToBasket2.length; j++) {

        totalAmount += productsToBasket2[j].price
    }
    document.getElementById("totalAmount").innerHTML = `${totalAmount}$`;
    document.getElementById("itemCount").innerHTML = parseInt(products.length) ;
}

const showProductBasket = async (product) => {
    const inbasket = await fetch(`https://localhost:44379/api/products/${product}`, {
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

    let productsToBasket2 = JSON.parse(sessionStorage.getItem("basketTwo"))
    productsToBasket2.push(shopingBag)
    sessionStorage.setItem("basketTwo", JSON.stringify(productsToBasket2))

    showOneProduct(shopingBag);
}

const showOneProduct = async (product) => {
    const url = `../images/${product.image}`
    let tmp = document.getElementById("temp-row");
    let cloneProduct = tmp.content.cloneNode(true)
    cloneProduct.querySelector(".image").style.backgroundImage = `url(${url})`
    cloneProduct.querySelector(".availabilityColumn").innerText = true;
    cloneProduct.querySelector(".itemName").textContent = product.productName
    cloneProduct.querySelector(".price").innerText = product.price
    cloneProduct.getElementById("delete").addEventListener('click', () => {
       deleteItem(product)
    })
    document.querySelector("tbody").appendChild(cloneProduct)
};



const detials = () => {
    const UserId1= JSON.parse(sessionStorage.getItem("user"))
    const UserId = UserId1.userId
    let orderItems1 = JSON.parse(sessionStorage.getItem("basket"))
    const OrderItems = []
    orderItems1.map(t => {
        let object = { productId: t, qantity: 1 }

        OrderItems.push(object)
    })
    let sum = orderItems1.length * 10
    let OrderDate = new Date()
    return ({
        OrderDate, sum, UserId, OrderItems
    })
}
const deleteItem = (product) => {
        products = JSON.parse(sessionStorage.getItem("basket"))
        let j = 0
        for (j = 0; j < products.length; j++) {

            if (products[j] == product.productId) {
                break;
            }
        }
        products.splice(j, 1)
        sessionStorage.setItem("basket", JSON.stringify(products))
        window.location.href = "ShoppingBag.html"
    showProductBasket()
}

const placeOrder = async () => {
    let alldetials = detials()
    const orderss = await fetch(`https://localhost:44379/api/Orders`, {
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




