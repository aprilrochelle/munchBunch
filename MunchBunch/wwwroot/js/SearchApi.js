$('#searchBtn').on("click", e => {
    e.preventDefault();
    const searchTerms = $('#searchQuery').val();
    const userLocation = $('#userLoc').val();

    let entityId = "";

    if (userLocation === "NASHVILLE") {
        entityId = 1138;
    } else if (userLocation === "MEMPHIS") {
        entityId = 1144;
    } else if (userLocation === "NEW ORLEANS") {
        entityId = 290;
    } else if (userLocation === "ATLANTA") {
        entityId = 288;
    } else {
      alert('Please enter a valid city name in your User Profile.');
    }

      fetch(`https://developers.zomato.com/api/v2.1/search?entity_id=${entityId}&entity_type=city&q=${searchTerms}`, {
          "headers": {
              "user-key": `${apiKey.Key}`,
              "Content-Type": "application/json"
          }
      })
        .then(res => res.json())
        .then(restaurants => {
            const select = document.querySelector("#results");
            let resultsHtml = "";

            restaurants.restaurants.map(restaurant => {
                resultsHtml += `<div class="restCard" id="${restaurant.restaurant.R.res_id}"><h3 class="restName">${restaurant.restaurant.name}</h3><h4>${restaurant.restaurant.location.locality}</h4><p>${restaurant.restaurant.location.address}</p>
                  <form class="searchForm" action="http://localhost:5000/memoirs/create" method="POST">
                    <input type="hidden" name="RId" value="${restaurant.restaurant.R.res_id}" />
                    <input type="hidden" name="RestaurantName" value="${restaurant.restaurant.name}" />
                    <input type="hidden" name="RestaurantLocation" value="${restaurant.restaurant.location.locality}" />
                    <input type="hidden" name="RestaurantAddress" value="${restaurant.restaurant.location.address}" />
                   <button class="ate">Ate It!</button>
                  </form>
                  <form class="searchForm" action="http://localhost:5000/wishlists/create" method="POST">
                    <input type="hidden" name="RId" value="${restaurant.restaurant.R.res_id}" />
                    <input type="hidden" name="RestaurantName" value="${restaurant.restaurant.name}" />
                    <input type="hidden" name="RestaurantLocation" value="${restaurant.restaurant.location.locality}" />
                    <input type="hidden" name="RestaurantAddress" value="${restaurant.restaurant.location.address}" />
                    <button class="want">Gotta Try!</button>
                  </form>
                  </div>`;

            });

            if (resultsHtml === "") {
              resultsHtml += `<h4 class="text-center">No results found. Try your search again.</h4>`;
            }

            select.innerHTML = resultsHtml;

        });

});