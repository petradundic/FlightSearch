import React, { useState } from "react";
import SearchForm from "./Components/SearchForm.jsx"; 
import FlightList from "./Components/FlightList.jsx"; 

const App = () => {
    const [flights, setFlights] = useState([]); 

    return (
        <div>
            <SearchForm setFlights={setFlights} />
            <FlightList flights={flights} />
        </div>
    );
};

export default App;
