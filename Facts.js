import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './Fact.css'; // Import the CSS file for styling

const Facts = () => {
    const [facts, setFacts] = useState([]);
    const [selectedFact, setSelectedFact] = useState(null);
    const [factText, setFactText] = useState('');

    useEffect(() => {
        fetchFacts();
    }, []);

    const fetchFacts = () => {
        axios.get('https://localhost:7166/api/facts')
            .then(response => {
                setFacts(response.data);
            })
            .catch(error => {
                console.error('There was an error fetching the facts!', error);
            });
    };

    const fetchFactById = (id) => {
        axios.get(`https://localhost:7166/api/facts/${id}`)
            .then(response => {
                setSelectedFact(response.data);
                setFactText(response.data.facts);
            })
            .catch(error => {
                console.error('There was an error fetching the fact by ID!', error);
            });
    };

    const addFact = () => {
        const newFact = { facts: factText };
        axios.post('https://localhost:7166/api/facts', newFact)
            .then(response => {
                fetchFacts();
                setSelectedFact(response.data);
                setFactText('');
            })
            .catch(error => {
                console.error('There was an error adding the fact!', error);
            });
    };

    const updateFact = () => {
        if (!selectedFact) return;

        const updatedFact = { ...selectedFact, facts: factText };
        axios.put(`https://localhost:7166/api/facts/${selectedFact.factId}`, updatedFact)
            .then(() => {
                fetchFacts();
                setSelectedFact(null);
                setFactText('');
            })
            .catch(error => {
                console.error('There was an error updating the fact!', error);
            });
    };

    const deleteFact = (id) => {
        axios.delete(`https://localhost:7166/api/facts/${id}`)
            .then(() => {
                fetchFacts();
            })
            .catch(error => {
                console.error('There was an error deleting the fact!', error);
            });
    };

    const resetForm = () => {
        setSelectedFact(null);
        setFactText('');
    };

    return (
        <div className="facts-container">
            <h2>Facts</h2>
            <ul>
                {facts.map(fact => (
                    <li key={fact.factId}>
                        {fact.facts}
                        <button onClick={() => fetchFactById(fact.factId)}>Edit</button>
                        <button onClick={() => deleteFact(fact.factId)}>Delete</button>
                    </li>
                ))}
            </ul>
            <div className="edit-form">
                <h3>{selectedFact ? 'Edit Fact' : 'Add Fact'}</h3>
                <label>Fact:</label>
                <textarea value={factText} onChange={(e) => setFactText(e.target.value)}></textarea>
                <button onClick={selectedFact ? updateFact : addFact}>
                    {selectedFact ? 'Update' : 'Add'}
                </button>
                {selectedFact && <button onClick={resetForm}>Cancel</button>}
            </div>
            {selectedFact && (
                <div className="fact-details">
                    <h3>Fact Details</h3>
                    <p>{selectedFact.facts}</p>
                </div>
            )}
        </div>
    );
};

export default Facts;
