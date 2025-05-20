async function DeleteUser(userId) {
    if (confirm('Are you sure you want to delete this user?')) {
        const response = await fetch(`/Users/${userId}`, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json'
            }
        });
        if (response.ok) {
            console.log("asdasd");
            //window.location.href = '/Users';
        } else {
            alert('Error deleting user.');
        }
    }
}