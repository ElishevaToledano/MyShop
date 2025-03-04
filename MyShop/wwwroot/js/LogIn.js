const strengthMeter = document.getElementById("strengthMeter");
strengthMeter.value = 0;

const welcome = () => {
    const currentUser = JSON.parse(sessionStorage.getItem("user"))
    if (currentUser != null)
        document.querySelector("#newUser").style.display = "none";
}
welcome();
const getAllDetilesForLogin = () => {
    return {
        userName: document.querySelector("#userNameLogin").value,
        password: document.querySelector("#passwordLogin").value,
    };
};

const getAllDetilesForSignUp = () => {
  
        const UserName=  document.querySelector("#userName").value
        const Password= document.querySelector("#password").value
        const FirstName= document.querySelector("#firstName").value
        const LastName= document.querySelector("#lastName").value
    return { UserName, Password, FirstName, LastName };
};

const checkData = (user) => {
    return (user.userName && user.password);
};

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

            if (responsePost.status === 400)
                throw new Error("all fields are request");
            else if (responsePost.status === 401)
                throw new Error("one of he fields are not valid");
            else { 
                throw new Error("Something went wrong, try again");
            }
        }
        else {
            alert("Password is strong enough!");

        }


    }
    catch (error) {
        alert(error.message);
    }
};
/////////////////

const addNewUser = async () => {
    const newUser = getAllDetilesForSignUp();
    if (strengthMeter.value < 3)
        alert("Password is too weak")
    else { 
    try {
        const responsePost = await fetch(`https://localhost:44379/api/Users`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(newUser)
        });

        if (responsePost.status === 400)
            throw new Error("All fields are required");

        if (!responsePost.ok)
            throw new Error("Something went wrong, try again");

        alert("User registered successfully!");
        showLogin(); // Return to login view after successful registration

    } catch (error) {
        alert(error);
    }
}
}
const showRegister = () => {
    document.querySelector(".logInDiv").style.display = "none";
    document.querySelector(".signUpDiv").classList.add("show");
    document.querySelector("#newUser").style.display = "none";
};

const showLogin = () => {
    document.querySelector(".logInDiv").style.display = "block";
    document.querySelector(".signUpDiv").classList.remove("show");
    document.querySelector("#newUser").style.display = "block";
};

const Login = async () =>
{
    const user = getAllDetilesForLogin();
    if (!checkData(user)) {
        alert("All fields are required");
        return;
    }

    try {
        const responsePost = await fetch(`https://localhost:44379/api/Users/login?userName=${user.userName}&password=${user.password}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });


        //    method: "POST",
        //    headers: {
        //        "Content-Type": "application/json"
        //    }
        //});

        if (responsePost.status == 204)
            alert("User not found")
        if (!responsePost.ok)
            alert("Eror,please try again")
        else
        { 
        const dataPost = await responsePost.json();
        sessionStorage.setItem("user", JSON.stringify(dataPost));
        window.location.href = "../html/Products.html";
        }

    }
    catch (error)
    {
        alert(error);

    }



}




