import { useState } from 'react';

export function OptionsSection({page, onSortOptionChangeClick}) {
    const [selectedOption, setSelectedOption] = useState(0);

    const optionsActivities = [
        { name: "LatestAdded", displayName:"Latest added" },
        { name: "LatestOccurred", displayName:"Latest" },
        { name: "MostLiked", displayName:"Most respected" },
        { name: "LeastLiked", displayName:"Least respected" },
        { name: "Trending", displayName:"Trending" },
        //  todo { name: "BestMatching", displayName:"Best matching" },
    ];

    const optionsPersons = [
        { name: "MostRespected", displayName:"Most Respected" },
        { name: "LeastRespected" , displayName:"Worst" },
        { name: "LatestAdded" , displayName:"Latest added" },
        { name: "AlphabeticalLastname", displayName:"Alphabetical A-z" },
        { name: "AlphabeticalReversedLastname", displayName:"Alphabetical Z-a" },
        { name: "Trending", displayName:"Trending" },
    ];

    const optionsComments = [
        { name: "MostRespected", displayName:"Most Respected" },
        { name: "LeastRespected" , displayName:"Worst" },
        { name: "LatestAdded" , displayName:"Latest added" },
        { name: "OldesttAdded" , displayName:"Oldest added" },
    ];

    let options = {};
    if(page === "Activities") {
        options = optionsActivities;
    } else if (page === "Persons") {
        options = optionsPersons;
    } else if(page === "Comments") {
        options = optionsComments;
    } else {
        options = [{displayName: "Error loading sorting options."}]
    }

    const onOptionClick = (options, index) => {
        console.log("   BrowseSection: onOptionClick(" + index + ")");
        onSortOptionChangeClick(options[index].name, options[index].displayName);
        setSelectedOption(index); 
    };

    return (
        <ul>
            {options.map((option, index) => (
                <li
                    key={index}
                    className={selectedOption === index  ? "selected-option" : ""}
                    onClick={() => onOptionClick(options, index)}
                >
                    {option.displayName}
                </li>
            ))}
        </ul>
    );
}