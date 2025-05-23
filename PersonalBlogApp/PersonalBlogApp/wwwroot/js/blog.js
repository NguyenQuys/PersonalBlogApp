async function DeleteBlog(blogId) {
    if (confirm('Are you sure you want to delete this blog post?')) {
        const response = await fetch(`/Blogs/${blogId}`, {
            method: 'DELETE'
        });
        if (response.ok) {
            window.location.href = '/Blogs/Manage';
        } else {
            console.error('Failed to delete the blog post.');
        }
    }
}