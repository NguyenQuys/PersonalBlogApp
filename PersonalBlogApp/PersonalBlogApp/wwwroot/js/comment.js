async function AddComment() {
    const content = $('#comment-input').val();
    const blogId = $('#blog-id').val();

    const formData = new FormData();
    formData.append("Content", content);
    formData.append("BlogId", blogId);

    try {
        const response = await fetch("/Comments", {
            method: 'POST',
            body: formData
        });

        const result = await response.json();
        if (result.status === 201) {
            $('#noComment').remove();

            const userName = result.result.username;
            const commentContent = result.result.content;
            const createdDate = new Date(result.result.createdDate).toLocaleString();

            const li = document.createElement('li');
            li.className = 'list-group-item border-success';
            li.id = `comment-${result.result.id}`
            li.innerHTML = `<div class="row align-items-center">
                                <div class="col-12 col-lg-11 col-md-10 d-flex justify-content-between align-items-center">
                                    <span><strong class="username-response">${userName}</strong>: ${commentContent}</span>
                                    <span class="text-muted ms-2">${createdDate}</span>
                                </div>
                                <div class="col-12 col-lg-1 col-md-2 d-flex flex-column flex-lg-row align-items-end justify-content-end mt-lg-0">
                                    <div class="dropdown">
										<button class="btn btn-link dropdown-toggle" type="button" id="commentActionsDropdown-${result.result.id}" data-bs-toggle="dropdown" aria-expanded="false">
											More
										</button>
										<ul class="dropdown-menu dropdown-menu-end" aria-labelledby="commentActionsDropdown-${result.result.id}">
											<li>
												<form action="/Comments/Delete" method="post" class="dropdown-item p-0 m-0">
													<input type="hidden" name="blogId" value="${result.result.blogId}" />
													<input type="hidden" name="commentId" value="${result.result.id}" />
													<button type="submit" class="btn btn-link w-100 text-start text-black text-decoration-none">Delete</button>
												</form>
											</li>
											<li>
												<button type="button" onclick="OpenReplyInput('${result.result.id}', '${result.result.content}')" class="dropdown-item">Reply</button>
											</li>
										</ul>
									</div>
                                </div>
                            </div>
                            <ul class="list-group mt-2 ms-4" id="reply-list-${result.result.id}"></ul>
                            `

            const commentList = document.getElementById('comment-list');
            if (commentList.firstChild) {
                commentList.insertBefore(li, commentList.firstChild);
            } else {
                commentList.appendChild(li);
            }

            const br = document.createElement('br');
            if (li.nextSibling) {
                commentList.insertBefore(br, li.nextSibling);
            } else {
                commentList.appendChild(br);
            }

            document.getElementById('comment-input').value = '';

        } else {
            console.log('Failed to add comment: ' + (result.message || 'Unknown error'));
        }
    } catch (error) {
        console.error('Error:', error);
    }
}

async function OpenReplyInput(parentCommentId, cmtContentParent) {
    const existingInput = document.getElementById('active-reply-input');
    if (existingInput) {
        existingInput.remove();
    }

    const divInput = document.createElement('div');
    divInput.className = 'input-group my-2';
    divInput.id = 'active-reply-input';
    divInput.innerHTML = `
        <input type="text" class="form-control" placeholder="Reply to ${cmtContentParent}..." id="reply-input">
        <button class="btn btn-primary" type="button" onclick="SendReply('${parentCommentId}')">Send</button>
    `;

    document.getElementById(`reply-list-${parentCommentId}`).appendChild(divInput);
}

async function SendReply(parentCommentId) {
    const blogId = $('#blog-id').val();
    const replyContent = $('#reply-input').val();

    const formData = new FormData();
    formData.append("Content", replyContent);
    formData.append("BlogId", blogId);
    formData.append('ParentCommentId', parentCommentId);

    const response = await fetch("/Comments", {
        method: 'POST',
        body: formData
    });

    if (response.ok) {
        const result = await response.json()
        const createdDate = new Date(result.result.createdDate).toLocaleString();

        let replyList = document.getElementById(`reply-list-${parentCommentId}`);

        if (!replyList) {
            replyList = document.createElement('ul');
            replyList.className = 'list-group mt-2 ms-4';
            replyList.id = `reply-list-${parentCommentId}`;

            const parentCommentElement = document.getElementById(`comment-${parentCommentId}`);
            parentCommentElement.appendChild(replyList);
        }

        const li = document.createElement('li');
        li.className = 'list-group-item';
        li.innerHTML = `
            <div>
                <strong class="username-response">${result.result.username}</strong>: ${result.result.content}
                <form method='post' class='d-inline' action='/Comments/Delete'>
                    <input type='hidden' name='blogId' value='${result.result.blogId}'>
                    <input type='hidden' name='commentId' value='${result.result.id}'>
                    <button type='submit' style='float:right' class='btn btn-link btn-sm ms-4'>Delete</button>
                </form>
                <span class="text-muted" style="float:right;">${createdDate}</span>
            </div>
        `;
        replyList.appendChild(li);

        // delete after send reply
        const inputDiv = document.getElementById('active-reply-input');
        if (inputDiv) inputDiv.remove();
    }
}



