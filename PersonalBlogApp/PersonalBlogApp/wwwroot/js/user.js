async function DeleteUser(userId) {
    if (confirm('Are you sure you want to delete this user?')) {
        const response = await fetch(`/Users/${userId}`, {
            method: 'DELETE',
        });
        
        if (response.ok) {
            window.location.href = '/Users';
        } else {
            alert('Error deleting user.');
        }
    }
}
