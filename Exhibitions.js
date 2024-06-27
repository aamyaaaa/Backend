import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './Exhibitions.css'; // Import the CSS file for styling

const Exhibitions = () => {
    const [exhibitions, setExhibitions] = useState([]);
    const [selectedExhibition, setSelectedExhibition] = useState(null);
    const [tourName, setTourName] = useState('');
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');
    const [isEditing, setIsEditing] = useState(false);
    const [isAdding, setIsAdding] = useState(false);

    useEffect(() => {
        fetchExhibitions();
    }, []);

    const fetchExhibitions = () => {
        axios.get('https://localhost:7166/api/exhibitiontours')
            .then(response => {
                setExhibitions(response.data);
            })
            .catch(error => {
                console.error('There was an error fetching the exhibitions!', error);
            });
    };

    const fetchExhibitionById = (id) => {
        axios.get(`https://localhost:7166/api/exhibitiontours/${id}`)
            .then(response => {
                const tour = response.data;
                setSelectedExhibition(tour);
                setTourName(tour.tourName);
                setStartDate(tour.startDate);
                setEndDate(tour.endDate);
                setIsEditing(true);
                setIsAdding(false);
            })
            .catch(error => {
                console.error('There was an error fetching the exhibition by ID!', error);
            });
    };

    const addExhibition = () => {
        const newExhibition = { tourName, startDate, endDate };
        axios.post('https://localhost:7166/api/exhibitiontours', newExhibition)
            .then(response => {
                fetchExhibitions();
                setTourName('');
                setStartDate('');
                setEndDate('');
                setIsAdding(false);
            })
            .catch(error => {
                console.error('There was an error adding the exhibition!', error);
            });
    };

    const updateExhibition = () => {
        if (!selectedExhibition) return;

        const updatedExhibition = { ...selectedExhibition, tourName, startDate, endDate };
        axios.put(`https://localhost:7166/api/exhibitiontours/${selectedExhibition.tourId}`, updatedExhibition)
            .then(() => {
                fetchExhibitions();
                setSelectedExhibition(null);
                setTourName('');
                setStartDate('');
                setEndDate('');
                setIsEditing(false);
            })
            .catch(error => {
                console.error('There was an error updating the exhibition!', error);
            });
    };

    const deleteExhibition = (id) => {
        axios.delete(`https://localhost:7166/api/exhibitiontours/${id}`)
            .then(() => {
                fetchExhibitions();
            })
            .catch(error => {
                console.error('There was an error deleting the exhibition!', error);
            });
    };

    const resetForm = () => {
        setSelectedExhibition(null);
        setTourName('');
        setStartDate('');
        setEndDate('');
        setIsEditing(false);
        setIsAdding(false);
    };

    return (
        <div className="exhibitions-container">
            <h2>Exhibition Tours</h2>
            <ul>
                {exhibitions.map(exhibition => (
                    <li key={exhibition.tourId}>
                        {exhibition.tourName} - {new Date(exhibition.startDate).toLocaleDateString()}
                        <button onClick={() => fetchExhibitionById(exhibition.tourId)}>Edit</button>
                        <button onClick={() => deleteExhibition(exhibition.tourId)}>Delete</button>
                    </li>
                ))}
            </ul>
            <div className="edit-form">
                    <h3>{isEditing ? 'Edit Exhibition' : 'Add Exhibition'}</h3>
                    <label>Tour Name:</label>
                    <input type="text" value={tourName} onChange={(e) => setTourName(e.target.value)} />
                    <label>Start Date:</label>
                    <input type="date" value={startDate} onChange={(e) => setStartDate(e.target.value)} />
                    <label>End Date:</label>
                    <input type="date" value={endDate} onChange={(e) => setEndDate(e.target.value)} />
                    <button onClick={isEditing ? updateExhibition : addExhibition}>
                        {isEditing ? 'Update' : 'Add'}
                    </button>
                    <button onClick={resetForm}>Cancel</button>
                </div>
        </div>
    );
};

export default Exhibitions;
