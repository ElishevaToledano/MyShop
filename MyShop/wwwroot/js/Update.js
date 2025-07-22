
const start = addEventListener("load", async () => {
    const userName = document.querySelector("#userNameUpdate")
/*    const password = document.querySelector("#password")*/
    const firstName = document.querySelector("#firstNameUpdate")
    const lastName = document.querySelector("#lastNameUpdate")
    const currentUser = JSON.parse(sessionStorage.getItem("user"))
    userName.value = currentUser.userName
    firstName.value = currentUser.firstName
    lastName.value = currentUser.lastName
})

const strengthMeter = document.getElementById("strengthMeter");
strengthMeter.value = 0;

//CHECK PASSWORD

const meterColor = (resData) => {
    strengthMeter.value = resData;
} 


// Function to get password details
const getDetailsOfPassword = () => {
    const password = document.getElementById("password").value;
    return password;
}

// Function to check password strength
const checkPassword = async () => {
    const password = getDetailsOfPassword();

    try {
        const responsePost = await fetch(`https://localhost:44379/api/Users/password`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(password)
        });
        const responseData = await responsePost.json();
        meterColor(responseData);
        if (!responsePost.ok) {

            if (responsePost.status === 400) throw new Error("Password is too weak");
            throw new Error("Something went wrong, try again");
        }
        else {
            alert("Password is strong enough!");

        }


    }
    catch (error) {
        alert(error.message);
    }
};


//const getAllDetilesForUpdate = () => {
//    const userName = document.querySelector("#userNameUpdate")
//    const password = document.querySelector("#password")
//    const firstName = document.querySelector("#firstNameUpdate")
//    const lastName = document.querySelector("#lastNameUpdate")
//    const currentUser = JSON.parse(sessionStorage.getItem("user"))
//    userName.value = currentUser.userName
//    firstName.value = currentUser.firstName
//    lastName.value = currentUser.lastName

//    return newUser = {
//        UserName: userName,
//        Password: password,
//        FirstName: firstName,
//        LastName: lastName
//    }
//    /*checkPassword();*/
//}


//editValueOfUpdatePage()
const getAllDetilesForUpdate = () => {
    return newUser = {
        UserName: document.querySelector("#userNameUpdate").value,
        Password: document.querySelector("#password").value,
        FirstName: document.querySelector("#firstNameUpdate").value,
        LastName: document.querySelector("#lastNameUpdate").value
    }
}

const updateUser = async () => {
    const updateUser = getAllDetilesForUpdate()
    const currentUser = JSON.parse(sessionStorage.getItem("user"))
    if (strengthMeter.value < 3)
        alert("Password is too weak")
    else {
        try {

            const responseput = await fetch(`https://localhost:44379/api/Users/${currentUser.userId}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(updateUser)
            })
            if (!responseput.ok)
                throw new Error(`HTTP error! status ${responsePost.status}`)


            if (responseput.status == 200) {
                alert(`The user  ${currentUser.firstName}  details updated successfully!`)
                window.location.href = "/html/Products.html";
            }
        }
        catch (error) {
            alert("Something went wrong, try again...\nThe error:" + error)
        }
    }
}