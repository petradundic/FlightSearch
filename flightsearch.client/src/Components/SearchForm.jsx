import React, { useState } from "react";
import PropTypes from "prop-types";
import axios from "axios";
import airport from "@nwpr/airport-codes";
import "bootstrap/dist/css/bootstrap.min.css";
import 'bootstrap-icons/font/bootstrap-icons.css';

const SearchForm = ({ setFlights }) => {
    const [searchParams, setSearchParams] = useState({});
    const [originSuggestions, setOriginSuggestions] = useState([]);
    const [destinationSuggestions, setDestinationSuggestions] = useState([]);


    const handleChange = (e) => {
        const { name, value } = e.target;
        setSearchParams({ ...searchParams, [name]: value });
    };

    const filterAirports = (input) => {
        if (!input) return [];
        const safeInput = input.toLowerCase();

        return (airport || []).filter(
            (a) =>
                a?.name?.toLowerCase().includes(safeInput) ||
                a?.iata?.toUpperCase().startsWith(input.toUpperCase()) ||
                a?.city?.toLowerCase().includes(safeInput) ||
                a?.country?.toLowerCase().includes(safeInput)
        );
    };

    const handleOriginInputChange = (e) => {
        const input = e.target.value;
        setSearchParams({ ...searchParams, origin: input });
        setOriginSuggestions(filterAirports(input));
    };

    const handleDestinationInputChange = (e) => {
        const input = e.target.value;
        setSearchParams({ ...searchParams, destination: input });
        setDestinationSuggestions(filterAirports(input));
    };

    const handleOriginSelect = (airport) => {
        setSearchParams({ ...searchParams, origin: `${airport.iata} - ${airport.city}, ${airport.country}` });
        setOriginSuggestions([]);
    };

    const handleDestinationSelect = (airport) => {
        setSearchParams({
            ...searchParams,
            destination: `${airport.iata} - ${airport.city}, ${airport.country}`,
        });
        setDestinationSuggestions([]);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!searchParams.origin || !searchParams.destination || !searchParams.departureDate) {
            alert("Please fill in all required fields: origin, destination, and departure date.");
            return;
        }

        const formattedParams = {
            Origin: searchParams.origin.split(" - ")[0], 
            Destination: searchParams.destination.split(" - ")[0],
            DepartureDate: `${searchParams.departureDate}T00:00:00`, 
            Passengers: searchParams.passengers || 1, 
            Currency: searchParams.currency || "EUR",
        };

        // Only add the return date if it's provided
        if (searchParams.returnDate) {
            formattedParams.ReturnDate = `${searchParams.returnDate}T00:00:00`;
        }
        console.log(formattedParams);
        try {
            const response = await axios.post(
                "https://localhost:7144/api/flight/search",
                formattedParams,
                {
                    headers: {
                        "Content-Type": "application/json",
                    },
                    withCredentials: true,
                }
            );
            console.log(response.data);
            setFlights(response.data);
        } catch (error) {
            console.error("Error fetching flights:", error.response?.data || error.message);
            if (error.response && error.response.status === 400) {
                alert(`Bad Request: ${error.response.data || 'Please check the search parameters.'}`);
            } else {
                alert("Error fetching flights. Please try again later.");
            }
        }
    };


    return (
        <div className="d-flex justify-content-center align-items-start pt-4">
            <form
                className="p-4 border rounded bg-white"
                style={{ width: "80vw", maxWidth: "1100px" }} 
                onSubmit={handleSubmit}
            >
                <h3 className="mb-4 text-center"><i className="bi bi-airplane"></i> Search Flights</h3>
                <div className="row">
                    <div className="col-md-3 mb-3 position-relative">
                        <label htmlFor="origin" className="form-label">
                            Origin
                        </label>
                        <input
                            type="text"
                            id="origin"
                            name="origin"
                            className="form-control"
                            value={searchParams.origin || ""}
                            onChange={handleOriginInputChange}
                            placeholder="Enter origin airport"
                            autoComplete="off"
                        />
                        {originSuggestions.length > 0 && (
                            <ul
                                className="list-group position-absolute"
                                style={{
                                    zIndex: 10,
                                    maxHeight: "150px",
                                    overflowY: "auto",
                                    width: "100%",
                                }}
                            >
                                {originSuggestions.map((a) => (
                                    <li
                                        key={a.id}
                                        className="list-group-item"
                                        onClick={() => handleOriginSelect(a)}
                                        style={{ cursor: "pointer" }}
                                    >
                                        {a.name} ({a.iata}) - {a.city}, {a.country}
                                    </li>
                                ))}
                            </ul>
                        )}
                    </div>

                    <div className="col-md-3 mb-3 position-relative">
                        <label htmlFor="destination" className="form-label">
                            Destination
                        </label>
                        <input
                            type="text"
                            id="destination"
                            name="destination"
                            className="form-control"
                            value={searchParams.destination || ""}
                            onChange={handleDestinationInputChange}
                            placeholder="Enter destination airport"
                            autoComplete="off"
                        />
                        {destinationSuggestions.length > 0 && (
                            <ul
                                className="list-group position-absolute"
                                style={{
                                    zIndex: 10,
                                    maxHeight: "150px",
                                    overflowY: "auto",
                                    width: "100%",
                                }}
                            >
                                {destinationSuggestions.map((a) => (
                                    <li
                                        key={a.id}
                                        className="list-group-item"
                                        onClick={() => handleDestinationSelect(a)}
                                        style={{ cursor: "pointer" }}
                                    >
                                        {a.name} ({a.iata}) - {a.city}, {a.country}
                                    </li>
                                ))}
                            </ul>
                        )}
                    </div>

                    <div className="col-md-2 mb-3">
                        <label htmlFor="departureDate" className="form-label">
                            Departure Date
                        </label>
                        <input
                            type="date"
                            id="departureDate"
                            name="departureDate"
                            className="form-control"
                            value={searchParams.departureDate || ""}
                            onChange={(e) => {
                                handleChange(e);
                                if (searchParams.returnDate && e.target.value > searchParams.returnDate) {
                                    setSearchParams({ ...searchParams, returnDate: "" });
                                }
                            }}
                        />
                    </div>

                    <div className="col-md-2 mb-3">
                        <label htmlFor="returnDate" className="form-label">
                            Return Date
                        </label>
                        <input
                            type="date"
                            id="returnDate"
                            name="returnDate"
                            className="form-control"
                            value={searchParams.returnDate || ""}
                            onChange={handleChange}
                            min={searchParams.departureDate || ""}
                        />
                    </div>
                </div>

                {/* Passengers and Currency */}
                <div className="row mb-3">
                    <div className="col-md-2">
                        <label htmlFor="passengers" className="form-label">
                            Passengers
                        </label>
                        <input
                            type="number"
                            id="passengers"
                            name="passengers"
                            className="form-control"
                            style={{ width: "8vw" }}
                            value={searchParams.passengers || 1}
                            min="1"
                            onChange={handleChange}
                        />
                    </div>
                    <div className="col-md-2">
                        <label htmlFor="currency" className="form-label">
                            Currency
                        </label>
                        <select
                            id="currency"
                            name="currency"
                            className="form-control"
                            style={{ width: "8vw" }}
                            value={searchParams.currency || "EUR"}
                            onChange={handleChange}
                        >
                            <option value="EUR">EUR</option>
                            <option value="USD">USD</option>
                            <option value="GBP">GBP</option>
                        </select>
                    </div>
                </div>

                <div className="text-center">
                    <button
                        type="submit"
                        className="btn btn-primary"
                        style={{ width: "10vw" }} 
                    >
                        Search
                    </button>
                </div>
            </form>
        </div>
    );
};

SearchForm.propTypes = {
    setFlights: PropTypes.func.isRequired,
};

export default SearchForm;
