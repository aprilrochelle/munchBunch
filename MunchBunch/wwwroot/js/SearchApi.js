$('#searchBtn').on("click", e => {
    e.preventDefault();
    const searchTerms = $('#searchQuery').val();

    fetch(`https://developers.zomato.com/api/v2.1/search?entity_id=1138&entity_type=city&q=${searchTerms}`, {
        "headers": {
            "user-key": `${apiKey.Key}`,
            "Content-Type": "application/json"
        }
    })
        .then(res => res.json())
        .then(restaurants => {
            const select = document.querySelector("#results");
            console.log(restaurants);
            let resultsHtml = "";

            restaurants.restaurants.map(restaurant => {
                resultsHtml += `<div id="${restaurant.restaurant.R.res_id}"><h3>${restaurant.restaurant.name}</h3><h4>${restaurant.restaurant.location.locality}</h4><p>${restaurant.restaurant.location.address}</p>
                  <form action="http://localhost:5000/memoirs/create" method="POST">
                    <input type="hidden" name="RId" value="${restaurant.restaurant.R.res_id}" />
                    <input type="hidden" name="RestaurantName" value="${restaurant.restaurant.name}" />
                    <input type="hidden" name="RestaurantLocation" value="${restaurant.restaurant.location.locality}" />
                    <input type="hidden" name="RestaurantAddress" value="${restaurant.restaurant.location.address}" />
                   <button class="ate btn-success">Ate It!</button>
                  </form>
                  <form action="http://localhost:5000/wishlists/create" method="POST">
                    <input type="hidden" name="RId" value="${restaurant.restaurant.R.res_id}" />
                    <input type="hidden" name="RestaurantName" value="${restaurant.restaurant.name}" />
                    <input type="hidden" name="RestaurantLocation" value="${restaurant.restaurant.location.locality}" />
                    <input type="hidden" name="RestaurantAddress" value="${restaurant.restaurant.location.address}" />
                    <button class="want btn-warning">Gotta Try!</button>
                  </form>
                  </div>`;

            });

            select.innerHTML = resultsHtml;

        });

});