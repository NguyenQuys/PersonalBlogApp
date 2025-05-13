

async function Register() {
    console.log($('#email-register').val());
    const formData = new FormData();

    formData.append("Email", $('#email-register').val());
    formData.append("UserName", document.getElementById("username-register").value);
    formData.append("Password", document.getElementById("password-register").value);
    formData.append("Avatar", document.getElementById("avatar-register").files[0]);
    
    try {
        const response = await fetch("/Auth/Register", {
            method: "POST",
            body: formData
        });

        if (response.ok) {
            const result = await response.json();
            console.log(result);
        } else {
            console.error("loi server:", response.statusText);
        }
    } catch (er) {
        console.error(er)
    }
}
