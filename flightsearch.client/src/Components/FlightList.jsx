import React from "react";
import PropTypes from "prop-types";
import "bootstrap/dist/css/bootstrap.min.css";

const FlightList = ({ flights }) => {
    return (
        <div className="d-flex justify-content-center pt-4">
            <div
                className="bg-white"
                style={{
                    maxWidth: "90vw",
                    width: "100%",
                    overflowY: "auto",
                    maxHeight: "50vh", 
                    height: "auto",
                }}
            >
                {flights.length === 0 ? (
                    <p className="text-center">No flights found.</p>
                ) : (
                    <ul className="list-group p-0">
                        {flights.map((flight) => (
                            <li
                                key={flight.id}
                                className="list-group-item mb-3 d-flex justify-content-between align-items-center"
                                style={{
                                    backgroundColor: "#f8f9fa",
                                    borderRadius: "8px", 
                                    padding: "1rem", 
                                    boxShadow: "0 2px 4px rgba(0, 0, 0, 0.1)",
                                }}
                            >
                                {/* Departure Section */}
                                <div style={{ flex: 1 }}>
                                    <h5>{flight.origin} to {flight.destination}</h5>
                                    <p>
                                        <strong>Departure:</strong> {new Date(flight.outboundDepartureTime).toLocaleString()}
                                    </p>
                                    <p>
                                        <strong>Arrival:</strong> {new Date(flight.outboundArrivalTime).toLocaleString()}
                                    </p>
                                    <p>
                                        <strong>Stops:</strong> {flight.outboundStops} {flight.outboundStops === 1 ? "Stop" : "Stops"}
                                    </p>
                                </div>

                                {/* Return Section */}
                                {flight.returnDepartureTime && flight.returnArrivalTime ? (
                                    <div style={{ flex: 1 }}>
                                        <h5 className="text-end">{flight.destination} to {flight.origin}</h5>
                                        <p className="text-end">
                                            <strong>Departure:</strong> {new Date(flight.returnDepartureTime).toLocaleString()}
                                        </p>
                                        <p className="text-end">
                                            <strong>Arrival:</strong> {new Date(flight.returnArrivalTime).toLocaleString()}
                                        </p>
                                        <p className="text-end">
                                            <strong>Stops:</strong> {flight.returnStops} {flight.returnStops === 1 ? "Stop" : "Stops"}
                                        </p>
                                    </div>
                                ) : (
                                    <div style={{ flex: 1 }}></div> 
                                )}

                                <div
                                    className="text-end"
                                    style={{
                                        fontWeight: "bold",
                                        flexBasis: "100px", 
                                    }}
                                >
                                    <p>
                                        Price: {flight.totalPrice} {flight.currency}
                                    </p>
                                </div>
                            </li>
                        ))}
                    </ul>
                )}
            </div>
        </div>
    );
};

FlightList.propTypes = {
    flights: PropTypes.arrayOf(
        PropTypes.shape({
            id: PropTypes.number.isRequired,
            origin: PropTypes.string.isRequired,
            destination: PropTypes.string.isRequired,
            outboundDepartureTime: PropTypes.string.isRequired,
            outboundArrivalTime: PropTypes.string.isRequired,
            returnDepartureTime: PropTypes.string,
            returnArrivalTime: PropTypes.string,
            outboundStops: PropTypes.number.isRequired,
            returnStops: PropTypes.number,
            passengers: PropTypes.number.isRequired,
            currency: PropTypes.string.isRequired,
            totalPrice: PropTypes.number.isRequired,
        })
    ).isRequired,
};

export default FlightList;
