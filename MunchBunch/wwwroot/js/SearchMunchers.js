$('#bunchSearchBtn').on("click", e => {
  e.preventDefault();
  const searchTerms = $('#userSearchQuery').val();
  console.log(searchTerms);

  fetch("http://localhost:5000/userfollows/index", {
    "searchTerm": `${searchTerms}`
  })
  .then(res => res.json())
  .then(users => {
    console.log(users);
    const select = document.querySelector("#bunchResults");
    let resultsHtml = "";

    users.map(user => {
      resultsHtml += `<div id="${user.id}" class="usersCard"><h3>this is a user card</h3><button class="btn ate"></button></div>`;
    });
    if (resultsHtml === "") {
      resultsHtml += `<h4 class="text-center">No results found. Try your search again.</h4>`;
    }

    select.innerHTML = resultsHtml;
  })
})