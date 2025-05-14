async function Register() {
    const formData = new FormData();
    const file = document.getElementById("avatar-register").files[0];

    formData.append("Email", $('#email-register').val());
    formData.append("UserName", $('#username-register').val());
    formData.append("PasswordHash", $('#password-register').val());
    formData.append("AvatarUrl", file);
    try {
        const response = await fetch("/Auth/Register", {
            method: 'POST',
            body: formData
        });

        const result = await response.json();
        if (response.statusCode === 201) {
            toastr.success(result.message);
            setTimeout(function () {
                window.location.href = "/Auth/Login";
            }, 2000); 
        } else {
            toastr.error(result.message);
        }
    } catch (err) {
        toastr.error(err);
    }
}

async function Login() {
    $('.loading').removeClass('d-none');

    const formData = new FormData();

    formData.append("UserName", $('#username-login').val());
    formData.append("PasswordHash", $("#password-login").val());

    try {
        const response = await fetch("/Auth/Login", {
            method: 'POST',
            body: formData
        });

        const result = await response.json();
        if (response.status === 200) {
            toastr.success(result.message);
            window.location.href = "/Home/Index"
        } else {
            toastr.error(result.message);
            $('.loading').addClass('d-none');
        }
    } catch (err) {
        toastr.error(err);
    }
}
