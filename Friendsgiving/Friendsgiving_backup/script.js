const names = ["Riley Harper", "Rohan Phadke", "Nakul Shah", "Mel Andrade-Muñoz", 
    "Bumika Vanapalli", "William Millen", "Ali Shamsulhodaey", "Sami Bommu", "Samy Vipin", 
    "Brenton Lian", "Kyle Chan", "Alice Kim", "Nava Mazumder", "Jessica Chen", 
    "Sanjana Nalla", "Sanjay Ravindran", "Rikhil Fellner", "Rebecca Henriques", 
    "Vihan Patil", "Anish Sharma", "Tiffany Meng", "Muhammad Fouly", "Nithya Mamalayan", 
    "Kush Gupta", "Yu Qi Lin", "Sebastian Ochenatu", "Andrew Abumoussa", "Dominique Higgins",
    "Yueh Lee", "Arryn O'Brien", "Ashley Neall"];

function filterNames() {
    let input = document.getElementById('nameInput');
    let filter = input.value.toUpperCase();
    let suggestions = document.getElementById('nameSuggestions');
    suggestions.innerHTML = '';

    if (!filter) {
        suggestions.style.display = "none";
        return;
    }

    names.forEach(function(name) {
        if (name.toUpperCase().indexOf(filter) > -1) {
            let div = document.createElement("div");
            div.innerHTML = name;
            div.onclick = function() {
                input.value = name;
                suggestions.style.display = "none";
            };
            suggestions.appendChild(div);
        }
    });
    suggestions.style.display = "block";
}

// Example Thanksgiving foods for autocomplete.
const thanksgivingFoods = [
    "Turkey",
    "Mashed Potatoes",
    "Gravy",
    "Stuffing",
    "Cranberry Sauce",
    "Green Bean Casserole",
    "Sweet Potato Casserole",
    "Pumpkin Pie",
    "Apple Pie",
    "Cornbread",
    "Ham",
    "Roast Beef",
    "Biscuits",
    "Macaroni and Cheese",
    "Dinner Rolls",
    "Sausage Gravy",
    "Brussels Sprouts",
    "Candied Yams",
    "Pecan Pie",
    "Cabbage",
    "Collard Greens",
    "Honey Glazed Carrots",
    "Chestnut Stuffing",
    "Pumpkin Cheesecake",
    "Candied Pecans",
    "Sweet Corn",
    "Cranberry Relish",
    "Deviled Eggs",
    "Garlic Mashed Potatoes",
    "Butternut Squash Soup",
    "Roasted Vegetables",
    "Chocolate Pecan Pie",
    "Creamed Spinach",
    "Green Salad",
    "Pumpkin Bread",
    "Cider",
    "Cherry Pie",
    "Creamed Corn",
    "Beef Gravy",
    "Broccoli Casserole",
    "Sweet Potato Pie",
    "Praline Sweet Potatoes",
    "Pumpkin Roll",
    "Roasted Turkey Breast",
    "Pumpkin Soup",
    "Pumpkin Bars",
    "Cranberry Pie",
    "Turkey Gravy",
    "Cornbread Stuffing",
    "Bourbon Pecan Pie",
    "Oyster Stuffing",
    "Pumpkin Spice Latte",
    "Apple Crisp",
    "Pumpkin Ice Cream",
    "Maple Glazed Ham",
    "Pumpkin Seeds",
    "Cranberry Juice",
    "Acorn Squash",
    "Candied Apples",
    "Pumpkin Pancakes",
    "Cranberry Muffins",
    "Pumpkin Cookies",
    "Pumpkin Fudge",
    "Turkey Sandwich",
    "Green Bean Almondine",
    "Pumpkin Soup",
    "Chocolate Truffles",
    "Caramel Apples",
    "Pumpkin Spice Cake",
    "Roasted Butternut Squash",
    "Apple Cider Donuts",
    "Sage Dressing",
    "Pumpkin Spice Waffles",
    "Chocolate Silk Pie",
    "Candied Pears",
    "Pumpkin Risotto",
    "Apple Cider Sangria",
    "Pear Pie",
    "Pumpkin Gnocchi",
    "Sausage and Mushroom Stuffing",
    "Apple Cobbler",
    "Pumpkin Curry",
    "Caramelized Onions",
    "Cranberry Vodka",
    "Pumpkin Ravioli",
    "Pomegranate Salad",
    "Pumpkin Creme Brulee",
    "Candied Walnuts",
    "Pumpkin Hummus",
    "Pumpkin Mousse",
    "Apple Slab Pie",
    "Pumpkin Cheesecake Bars",
    "Pumpkin Martini",
    "Cranberry Cheesecake",
    "Pumpkin Whoopie Pies",
    "Apple Turnovers",
    "Pumpkin Soup",
    "Cranberry Shortbread Cookies",
    "Pumpkin Chili",
    "Pumpkin Smoothie",
    "Apple Butter",
    "Pumpkin Scones",
    "Pumpkin Tiramisu",
    "Cranberry Lemonade",
    "Pumpkin Biscotti",
    "Pumpkin Flan",
    "Pumpkin Bruschetta",
    "Apple and Pear Crisp",
];

function filterFoods() {
    let input = document.getElementById('foodInput');
    let filter = input.value.toUpperCase();
    let suggestions = document.getElementById('foodSuggestions');
    suggestions.innerHTML = '';

    if (!filter) {
        suggestions.style.display = "none";
        return;
    }

    thanksgivingFoods.forEach(function(food) {
        if (food.toUpperCase().indexOf(filter) > -1) {
            let div = document.createElement("div");
            div.innerHTML = food;
            div.onclick = function() {
                input.value = food;
                suggestions.style.display = "none";
            };
            suggestions.appendChild(div);
        }
    });
    suggestions.style.display = "block";
}
